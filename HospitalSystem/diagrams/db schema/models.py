from django.db import models
from django.utils import timezone
from django.core.exceptions import ValidationError


# ──────────────────────────────────────────
# WARD
# ──────────────────────────────────────────
class Ward(models.Model):

    class WardType(models.TextChoices):
        GENERAL   = 'general',   'General'
        ICU       = 'icu',       'ICU'
        PEDIATRIC = 'pediatric', 'Pediatric'
        MATERNITY = 'maternity', 'Maternity'
        EMERGENCY = 'emergency', 'Emergency'

    ward_name = models.CharField(max_length=100)
    ward_type = models.CharField(max_length=20, choices=WardType.choices)
    capacity  = models.PositiveIntegerField()

    def str(self):
        return f"{self.ward_name} ({self.get_ward_type_display()})"

    def get_available_beds(self):
        """يرجع كل السراير المتاحة في الجناح"""
        return Bed.objects.filter(
            room__ward=self,
            current_status=Bed.BedStatus.AVAILABLE
        )

    def get_occupancy_rate(self):
        """يحسب نسبة الإشغال (عدد المحجوزة / الكل)"""
        total = Bed.objects.filter(room__ward=self).count()
        if total == 0:
            return 0.0
        occupied = Bed.objects.filter(
            room__ward=self,
            current_status=Bed.BedStatus.OCCUPIED
        ).count()
        return round((occupied / total) * 100, 2)

    def is_full(self):
        """يتحقق إذا الجناح ممتلئ — يمنع الـ overcrowding"""
        return self.get_available_beds().count() == 0

    class Meta:
        db_table = 'ward'


# ──────────────────────────────────────────
# ROOM
# ──────────────────────────────────────────
class Room(models.Model):

    ward        = models.ForeignKey(Ward, on_delete=models.CASCADE, related_name='rooms')
    room_number = models.CharField(max_length=20)
    floor       = models.PositiveIntegerField()

    def str(self):
        return f"Room {self.room_number} - Floor {self.floor}"

    def get_available_beds(self):
        """يرجع السراير المتاحة في الغرفة"""
        return self.beds.filter(current_status=Bed.BedStatus.AVAILABLE)

    def is_full(self):
        """يتحقق إذا الغرفة ممتلئة"""
        return self.get_available_beds().count() == 0

    class Meta:
        db_table = 'room'
        unique_together = ('ward', 'room_number')


# ──────────────────────────────────────────
# BED
# ──────────────────────────────────────────
class Bed(models.Model):

    class BedStatus(models.TextChoices):
        AVAILABLE      = 'available',      'Available'
        OCCUPIED       = 'occupied',       'Occupied'
        UNDER_CLEANING = 'under_cleaning', 'Under Cleaning'

    class BedType(models.TextChoices):
        STANDARD  = 'standard',  'Standard'
        ICU       = 'icu',       'ICU'
        PEDIATRIC = 'pediatric', 'Pediatric'
        MATERNITY = 'maternity', 'Maternity'

    room           = models.ForeignKey(Room, on_delete=models.CASCADE, related_name='beds')
    bed_number     = models.CharField(max_length=20)
    current_status = models.CharField(max_length=20, choices=BedStatus.choices, default=BedStatus.AVAILABLE)
    bed_type       = models.CharField(max_length=20, choices=BedType.choices)

    def str(self):
        return f"Bed {self.bed_number} [{self.get_current_status_display()}]"

    def _change_status(self, new_status):
        """دالة داخلية تغير الحالة وتسجل في Log تلقائياً"""
        old_status = self.current_status
        self.current_status = new_status
        self.save()
        BedStatusLog.objects.create(
            bed=self,
            old_status=old_status,
            new_status=new_status
        )

    def is_available(self):
        """يتحقق إذا السرير متاح"""
        return self.current_status == self.BedStatus.AVAILABLE

    def assign_to_patient(self):
        """يسند السرير لمريض — يغير الحالة لـ Occupied"""
        if not self.is_available():
            raise ValidationError(f"Bed {self.bed_number} is not available.")
        self._change_status(self.BedStatus.OCCUPIED)

    def mark_as_cleaning(self):
        """يحول السرير لـ Under Cleaning بعد خروج المريض"""
        if self.current_status != self.BedStatus.OCCUPIED:
            raise ValidationError(f"Bed {self.bed_number} is not occupied.")
        self._change_status(self.BedStatus.UNDER_CLEANING)

    def mark_as_available(self):
        """يحول السرير لـ Available بعد التنظيف"""
        if self.current_status != self.BedStatus.UNDER_CLEANING:
            raise ValidationError(f"Bed {self.bed_number} is not under cleaning.")
        self._change_status(self.BedStatus.AVAILABLE)

    class Meta:
        db_table = 'bed'
        unique_together = ('room', 'bed_number')


# ──────────────────────────────────────────
# BED STATUS LOG
# ──────────────────────────────────────────
class BedStatusLog(models.Model):

    bed        = models.ForeignKey(Bed, on_delete=models.CASCADE, related_name='status_logs')
    old_status = models.CharField(max_length=20, choices=Bed.BedStatus.choices)
    new_status = models.CharField(max_length=20, choices=Bed.BedStatus.choices)
    changed_at = models.DateTimeField(auto_now_add=True)

    def str(self):
        return f"Bed {self.bed_id}: {self.old_status} → {self.new_status} at {self.changed_at}"

    @classmethod
    def get_logs_for_bed(cls, bed_id):
        """يرجع كل سجلات تغيير حالة سرير معين"""
        return cls.objects.filter(bed_id=bed_id).order_by('-changed_at')

    def get_latest_status(self):
        """يرجع آخر حالة مسجلة للسرير"""
        return self.new_status

    class Meta:
        db_table = 'bed_status_log'
        ordering = ['-changed_at']


# ──────────────────────────────────────────
# PATIENT  (from Admission module)
# ──────────────────────────────────────────
class Patient(models.Model):

    class Gender(models.TextChoices):
        MALE   = 'male',   'Male'
        FEMALE = 'female', 'Female'

    full_name     = models.CharField(max_length=150)
    date_of_birth = models.DateField()
    gender        = models.CharField(max_length=10, choices=Gender.choices)

    def str(self):
        return self.full_name

    def get_admissions(self):
        """يرجع كل طلبات الدخول للمريض"""
        return self.admissions.all()

    def get_current_bed(self):
        """يرجع السرير الحالي للمريض إذا كان منوماً"""
        active = Hospitalization.objects.filter(
            admission__patient=self,
            status=Hospitalization.HospitalizationStatus.ACTIVE
        ).first()
        return active.bed if active else None

    class Meta:
        db_table = 'patient'


# ──────────────────────────────────────────
# ADMISSION  (from Admission module)
# ──────────────────────────────────────────
class Admission(models.Model):

    class AdmissionStatus(models.TextChoices):
        PENDING    = 'pending',    'Pending'
        ADMITTED   = 'admitted',   'Admitted'
        DISCHARGED = 'discharged', 'Discharged'
        CANCELLED  = 'cancelled',  'Cancelled'

    patient            = models.ForeignKey(Patient, on_delete=models.PROTECT, related_name='admissions')
    admission_date     = models.DateField()
    required_ward_type = models.CharField(max_length=20, choices=Ward.WardType.choices)
    required_bed_type  = models.CharField(max_length=20, choices=Bed.BedType.choices)
    admission_status   = models.CharField(max_length=20, choices=AdmissionStatus.choices, default=AdmissionStatus.PENDING)

    def str(self):
        return f"Admission #{self.pk} - {self.patient}"

    def validate_bed_match(self, bed):
        """
        يتحقق إن السرير المختار يناسب المتطلبات الطبية للمريض.
        هذا هو الـ validation الأساسي في الـ module.
        """
        if bed.bed_type != self.required_bed_type:
            raise ValidationError(
                f"Bed type mismatch: required {self.required_bed_type}, got {bed.bed_type}."
            )
        if bed.room.ward.ward_type != self.required_ward_type:
            raise ValidationError(
                f"Ward type mismatch: required {self.required_ward_type}, got {bed.room.ward.ward_type}."
            )
        return True
    def approve(self):
        """يغير حالة الطلب لـ Admitted"""
        self.admission_status = self.AdmissionStatus.ADMITTED
        self.save()

    def cancel(self):
        """يلغي طلب الدخول"""
        self.admission_status = self.AdmissionStatus.CANCELLED
        self.save()

    class Meta:
        db_table = 'admission'


# ──────────────────────────────────────────
# HOSPITALIZATION
# ──────────────────────────────────────────
class Hospitalization(models.Model):

    class HospitalizationStatus(models.TextChoices):
        ACTIVE      = 'active',      'Active'
        DISCHARGED  = 'discharged',  'Discharged'
        TRANSFERRED = 'transferred', 'Transferred'

    admission          = models.ForeignKey(Admission, on_delete=models.PROTECT, related_name='hospitalizations')
    bed                = models.ForeignKey(Bed, on_delete=models.PROTECT, related_name='hospitalizations')
    check_in_datetime  = models.DateTimeField()
    check_out_datetime = models.DateTimeField(null=True, blank=True)
    status             = models.CharField(max_length=20, choices=HospitalizationStatus.choices, default=HospitalizationStatus.ACTIVE)

    def str(self):
        return f"Hospitalization #{self.pk} - {self.admission.patient} in Bed {self.bed}"

    def get_duration(self):
        """
        يحسب عدد أيام الإقامة.
        يُستخدم كـ Quantity عند إرسال البيانات لـ Medical Services.
        """
        end = self.check_out_datetime or timezone.now()
        delta = end - self.check_in_datetime
        return max(delta.days, 1)  # minimum يوم واحد

    def discharge(self):
        """يسجل خروج المريض ويحول السرير لـ Under Cleaning"""
        if self.status != self.HospitalizationStatus.ACTIVE:
            raise ValidationError("Hospitalization is not active.")
        self.check_out_datetime = timezone.now()
        self.status = self.HospitalizationStatus.DISCHARGED
        self.save()
        self.bed.mark_as_cleaning()

    def transfer_bed(self, new_bed):
        """ينقل المريض لسرير آخر ويحدث السجلات"""
        self.admission.validate_bed_match(new_bed)
        old_bed = self.bed
        old_bed.mark_as_cleaning()
        new_bed.assign_to_patient()
        self.bed = new_bed
        self.status = self.HospitalizationStatus.TRANSFERRED
        self.save()

    def get_services(self):
        """يرجع كل الخدمات المرتبطة بهذه الإقامة"""
        return self.services.all()

    class Meta:
        db_table = 'hospitalization'
        ordering = ['-check_in_datetime']


# ──────────────────────────────────────────
# HOSPITALIZATION SERVICE
# ربط مع Medical Services Module
# ──────────────────────────────────────────
class HospitalizationService(models.Model):

    hospitalization = models.ForeignKey(
        Hospitalization,
        on_delete=models.PROTECT,
        related_name='services'
    )
    service_id    = models.IntegerField()            # من Medical Services Module
    invoice_id    = models.IntegerField()            # من Medical Services Module
    service_price = models.DecimalField(max_digits=10, decimal_places=2)
    quantity      = models.PositiveIntegerField()    # عدد الأيام = get_duration()

    def str(self):
        return f"Service {self.service_id} - Invoice {self.invoice_id}"

    def calculate_total(self):
        """يحسب إجمالي الخدمة: السعر × عدد الأيام"""
        return self.service_price * self.quantity

    @classmethod
    def create_from_hospitalization(cls, hospitalization, service_id, invoice_id, service_price):
        """
        ينشئ سجل خدمة تلقائياً عند إحالة المريض للسرير.
        يأخذ الـ quantity من get_duration() تلقائياً.
        """
        return cls.objects.create(
            hospitalization=hospitalization,
            service_id=service_id,
            invoice_id=invoice_id,
            service_price=service_price,
            quantity=hospitalization.get_duration()
        )

    class Meta:
        db_table = 'hospitalization_service'