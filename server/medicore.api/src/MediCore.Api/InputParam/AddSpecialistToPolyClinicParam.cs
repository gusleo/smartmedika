namespace MediCore.Api.InputParam
{
    /// <summary>
    /// Model view for add specialist to polyclinic
    /// </summary>
    public class AddSpecialistToPolyClinicParam
    {
        public int PolyClinicId { get; set; }
        public int[] SpecialistIds { get; set; }
    }
}
