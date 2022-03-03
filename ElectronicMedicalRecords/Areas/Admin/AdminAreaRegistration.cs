using System.Web.Mvc;

namespace ElectronicMedicalRecords.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }
         
        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "UpdateUser",
                "Admin/{id}-Cap-nhat-thong-tin-ca-nhan",
                new { controller = "Users", action = "Edit", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "CreateInfo",
                "Admin/{id}-Tao-Ho-So-Kham-Benh",
                new { controller = "MultipleModels", action = "CreateTest", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "EditByID",
                "Admin/{id}-Cap-Nhat-Ket-Qua-Xet-Nghiem",
                new { controller = "MultipleModels", action = "EditByID", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "DetailsIE",
                "Admin/{id}-Ket-Qua-Xet-Nghiem",
                new { controller = "MultipleModels", action = "DetailsIE", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "BillExamination",
                "Admin/{id}-Hoa-Don",
                new { controller = "MultipleModels", action = "BillExamination", id = UrlParameter.Optional }
            );


            context.MapRoute(
                "CreateOldPatient",
                "Admin/{id}-Tao-Ho-So-Benh-Nhan-Cu",
                new { controller = "MultipleModels", action = "CreateOldPatient", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "EditPatient",
                "Admin/{id}-Cap-Nhat-Benh-Nhan",
                new { controller = "MultipleModels", action = "Edit", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "HistoryInfomationOfPatient",
                "Admin/{id}-Lich-Su-Kham-Benh",
                new { controller = "InformationExaminations", action = "Details", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "DetailsIEReadInfomationOfPatient",
                "Admin/{id}-Chi-Tiet-Lich-Su",
                new { controller = "MultipleModels", action = "DetailsIERead", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "PrintTestCTMaus",
                "Admin/In-CDXN-Cong-Thuc-Mau",
                new { controller = "MultipleModels", action = "PrintTestCTMaus" }
            );

            context.MapRoute(
                "PrintTestSinhHoaMaus",
                "Admin/In-CDXN-Sinh-Hoa-Mau",
                new { controller = "MultipleModels", action = "PrintTestSinhHoaMaus" }
            );

            context.MapRoute(
                "PrintTestDongMaus",
                "Admin/In-CDXN-Dong-Mau",
                new { controller = "MultipleModels", action = "PrintTestDongMaus" }
            );

            context.MapRoute(
                "PrintTestNhomMaus",
                "Admin/In-CDXN-Nhom-Mau",
                new { controller = "MultipleModels", action = "PrintTestNhomMaus" }
            );

            context.MapRoute(
                "PrintTestNuocTieus",
                "Admin/In-CDXN-Nuoc-Tieu",
                new { controller = "MultipleModels", action = "PrintTestNuocTieus" }
            );

            context.MapRoute(
                "PrintTestMienDichs",
                "Admin/In-CDXN-Mien-Dich",
                new { controller = "MultipleModels", action = "PrintTestMienDichs" }
            );

            context.MapRoute(
                "PrintTestDichChocDos",
                "Admin/In-CDXN-Dich-Choc-Do",
                new { controller = "MultipleModels", action = "PrintTestDichChocDos" }
            );

            context.MapRoute(
                "PrintTestViSinhs",
                "Admin/In-CDXN-Vi-Sinh",
                new { controller = "MultipleModels", action = "PrintTestViSinhs" }
            );

            context.MapRoute(
                "PrintPrescriptions",
                "Admin/In-Toa-Thuoc",
                new { controller = "MultipleModels", action = "PrintPrescriptions" }
            );

            context.MapRoute(
                "PrintExaminationInfo",
                "Admin/In-Thong-Tin-Kham-Benh",
                new { controller = "MultipleModels", action = "PrintExaminationInfo" }
            );

            context.MapRoute(
                "PrintAllTestInfo",
                "Admin/In-Chi-Dinh-Xet-Nghiem",
                new { controller = "MultipleModels", action = "PrintAllTestInfo" }
            );

            context.MapRoute(
                "PrintAllExaminationInfo",
                "Admin/In-Ho-So-KQ-Xet-Nghiem",
                new { controller = "MultipleModels", action = "PrintAllExaminationInfo" }
            );

            context.MapRoute(
                "PrintResultCTMau",
                "Admin/In-KQXN-Cong-Thuc-Mau",
                new { controller = "MultipleModels", action = "PrintResultCTMau" }
            );

            context.MapRoute(
                "PrintResultSHMau",
                "Admin/In-KQXN-Sinh-Hoa-Mau",
                new { controller = "MultipleModels", action = "PrintResultSHMau" }
            );

            context.MapRoute(
                "PrintResultDongMau",
                "Admin/In-KQXN-Dong-Mau",
                new { controller = "MultipleModels", action = "PrintResultDongMau" }
            );

            context.MapRoute(
                "PrintResultNhomMau",
                "Admin/In-KQXN-Nhom-Mau",
                new { controller = "MultipleModels", action = "PrintResultNhomMau" }
            );

            context.MapRoute(
                "PrintResultNuocTieu",
                "Admin/In-KQXN-Nuoc-Tieu",
                new { controller = "MultipleModels", action = "PrintResultNuocTieu" }
            );

            context.MapRoute(
                "PrintResultMienDich",
                "Admin/In-KQXN-Mien-Dich",
                new { controller = "MultipleModels", action = "PrintResultMienDich" }
            );

            context.MapRoute(
                "PrintResultDichChocDo",
                "Admin/In-KQXN-Dich-Choc-Do",
                new { controller = "MultipleModels", action = "PrintResultDichChocDo" }
            );

            context.MapRoute(
                "PrintResultViSinh",
                "Admin/In-KQXN-Vi-Sinh",
                new { controller = "MultipleModels", action = "PrintResultViSinh" }
            );

            context.MapRoute(
                "SearchPatient",
                "Admin/Tim-Kiem-Benh-Nhan",
                new { controller = "Patients", action = "SearchPatient" }
            );

            context.MapRoute(
                "SearchPatientNoBook",
                "Admin/Tim-Kiem-Benh-Nhan-Khong-So",
                new { controller = "Patients", action = "SearchPatientNoInfo" }
            );

            context.MapRoute(
                "SearchPatientDetail",
                "Admin/Tim-Kiem-Lich-Su-Kham-Benh",
                new { controller = "InformationExaminations", action = "SearchPatientDetail" }
            );

            context.MapRoute(
                "SearchBSAndTT",
                "Admin/Thong-Ke-BS-Theo-Ngay",
                new { controller = "Statistics", action = "SearchBSAndTT" }
            );

            context.MapRoute(
                "SearchTT",
                "Admin/Thong-Ke-TT-Theo-Ngay",
                new { controller = "Statistics", action = "SearchTT" }
            );

            context.MapRoute(
                "SearchDiagnostic",
                "Admin/Thong-Ke-Nhom-Benh-Theo-Ngay",
                new { controller = "Statistics", action = "SearchDiagnostic" }
            );

            context.MapRoute(
                "SearchMoney",
                "Admin/Thong-Ke-Doanh-Thu-Theo-Ngay",
                new { controller = "Statistics", action = "SearchMoney" }
            );

            context.MapRoute(
                "PrintBillExamination",
                "Admin/In-Hoa-Don-Kham-Benh",
                new { controller = "Patients", action = "PrintBillExamination" }
            );

            context.MapRoute(
                "PrintBillTestSubclinical",
                "Admin/In-Hoa-Don-Xet-Nghiem",
                new { controller = "Patients", action = "PrintBillTestSubclinical" }
            );

            context.MapRoute(
                "PrintBillPrescription",
                "Admin/In-Hoa-Don-Thuoc",
                new { controller = "Patients", action = "PrintBillPrescription" }
            );

            context.MapRoute(
                "PrintStatisByDocandCon",
                "Admin/Thong-Ke-BS-Tinh-Trang-Benh-Nhan",
                new { controller = "Statistics", action = "PrintStatisByDocandCon" }
            );

            context.MapRoute(
                "PrintStatisticByCondition",
                "Admin/Thong-Ke-Tinh-Trang-Benh-Nhan",
                new { controller = "Statistics", action = "PrintStatisticByCondition" }
            );

            context.MapRoute(
                "PrintStatisticDiagnostic",
                "Admin/Thong-Ke-Nhom-Benh",
                new { controller = "Statistics", action = "PrintStatisticDiagnostic" }
            );

            context.MapRoute(
                "PrintStatisticMoney",
                "Admin/Thong-Ke-Doanh-Thu",
                new { controller = "Statistics", action = "PrintStatisticMoney" }
            );

            context.MapRoute(
                "HomePage",
                "Admin/Trang-Chu",
                new { controller = "Users", action = "HomePage" }
            );

            context.MapRoute(
                "DashBoard",
                "Admin/Dashboard",
                new { controller = "Dashboard", action = "Dashboard" }
            );

            context.MapRoute(
                "BillExam",
                "Admin/Hoa-Don-Kham-Benh",
                new { controller = "Statistics", action = "BillExam" }
            );

            context.MapRoute(
                "BillPre",
                "Admin/Hoa-Don-Thuoc",
                new { controller = "Statistics", action = "BillPre" }
            );

            context.MapRoute(
                "BillTest",
                "Admin/Hoa-Don-Xet-Nghiem",
                new { controller = "Statistics", action = "BillTest" }
            );

            context.MapRoute(
                "StatisByDoctorAndCondition",
                "Admin/Bac-Si",
                new { controller = "Statistics", action = "StatisByDoctorAndCondition" }
            );

            context.MapRoute(
                "StatisByCondition",
                "Admin/Tinh-Trang-Benh-Nhan",
                new { controller = "Statistics", action = "StatisByCondition" }
            );

            context.MapRoute(
                "StatisByDiagnostic",
                "Admin/Nhom-Benh",
                new { controller = "Statistics", action = "StatisByDiagnostic" }
            );

            context.MapRoute(
                "StatisByMoney",
                "Admin/Doanh-Thu",
                new { controller = "Statistics", action = "StatisByMoney" }
            );

            context.MapRoute(
                "CreatePatient",
                "Admin/Tao-Ho-So-Benh-Nhan",
                new { controller = "MultipleModels", action = "Create" }
            );

            context.MapRoute(
                "UserIndex",
                "Admin/Tai-Khoan",
                new { controller = "Users", action = "Index" }
            );

            context.MapRoute(
                "AspNetRoleIndex",
                "Admin/Phan-Quyen",
                new { controller = "AspNetRoles", action = "Index" }
            );

            context.MapRoute(
                "MedicationIndex",
                "Admin/Thuoc",
                new { controller = "Medications", action = "Index" }
            );

            context.MapRoute(
                "DiagnosticCategoryIndex",
                "Admin/Danh-Muc-Chuan-Doan",
                new { controller = "DiagnosticsCategories", action = "Index" }
            );

            context.MapRoute(
                "PatientIndex",
                "Admin/Danh-Sach-Benh-Nhan",
                new { controller = "Patients", action = "Index" }
            );

            context.MapRoute(
                "PrescriptionBillIndex",
                "Admin/Danh-Sach-Toa-Thuoc",
                new { controller = "Prescription_Detail", action = "ListPrescriptionBill" }
            );

            context.MapRoute(
                "ListExaminationIndex",
                "Admin/Lich-Su-Kham-Benh",
                new { controller = "Prescription_Detail", action = "ListExamination" }
            );

            context.MapRoute(
                "PatientStatusIndex",
                "Admin/Tinh-Trang-Benh-Nhan",
                new { controller = "PatientStatus", action = "Index" }
            );

            context.MapRoute(
                "MedicationCategoriesIndex",
                "Admin/Danh-Muc-Thuoc",
                new { controller = "MedicationCategories", action = "Index" }
            );

            context.MapRoute(
                "HistoryDiseasesIndex",
                "Admin/Benh-Tien-Su",
                new { controller = "HistoryDiseases", action = "Index" }
            );

            context.MapRoute(
                "MedicalHistoriesIndex",
                "Admin/Benh-Su",
                new { controller = "MedicalHistories", action = "Index" }
            );

            context.MapRoute(
                "AmniocentesIndex",
                "Admin/Xet-Nghiem-Dich-Choc-Do",
                new { controller = "Amniocentes", action = "Index" }
            );

            context.MapRoute(
                "DongMausIndex",
                "Admin/Xet-Nghiem-Dong-Mau",
                new { controller = "DongMaus", action = "Index" }
            );

            context.MapRoute(
                "ImmunesIndex",
                "Admin/Xet-Nghiem-Mien-Dich",
                new { controller = "Immunes", action = "Index" }
            );

            context.MapRoute(
                "NhomMausIndex",
                "Admin/Xet-Nghiem-Nhom-Mau",
                new { controller = "NhomMaus", action = "Index" }
            );

            context.MapRoute(
                "SinhHoaMausIndex",
                "Admin/Xet-Nghiem-Sinh-Hoa-Mau",
                new { controller = "SinhHoaMaus", action = "Index" }
            );

            context.MapRoute(
                "UrinesIndex",
                "Admin/Xet-Nghiem-Nuoc-Tieu",
                new { controller = "Urines", action = "Index" }
            );

            context.MapRoute(
                "ViSinhsIndex",
                "Admin/Xet-Nghiem-Vi-Sinh",
                new { controller = "ViSinhs", action = "Index" }
            );

            context.MapRoute(
                "TuanHoansIndex",
                "Admin/Benh-Tuan-Hoan",
                new { controller = "TuanHoans", action = "Index" }
            );

            context.MapRoute(
                "HoHapsIndex",
                "Admin/Benh-Ho-Hap",
                new { controller = "HoHaps", action = "Index" }
            );

            context.MapRoute(
                "TieuHoasIndex",
                "Admin/Benh-Tieu-Hoa",
                new { controller = "TieuHoas", action = "Index" }
            );

            context.MapRoute(
                "ThanTietNieuxIndex",
                "Admin/Benh-Than-Tiet-Nieu",
                new { controller = "ThanTietNieux", action = "Index" }
            );

            context.MapRoute(
                "CoXuongKhopsIndex",
                "Admin/Benh-Co-Xuong-Khop",
                new { controller = "CoXuongKhops", action = "Index" }
            );

            context.MapRoute(
                "ThanKinhsIndex",
                "Admin/Benh-Than-Kinh",
                new { controller = "ThanKinhs", action = "Index" }
            );

            context.MapRoute(
                "TamThansIndex",
                "Admin/Benh-Tam-Than",
                new { controller = "TamThans", action = "Index" }
            );

            context.MapRoute(
                "NgoaiKhoasIndex",
                "Admin/Benh-Ngoai-Khoa",
                new { controller = "NgoaiKhoas", action = "Index" }
            );

            context.MapRoute(
                "SanPhuKhoasIndex",
                "Admin/Benh-San-Phu-Khoa",
                new { controller = "SanPhuKhoas", action = "Index" }
            );

            context.MapRoute(
                "MatsIndex",
                "Admin/Benh-Mat",
                new { controller = "Mats", action = "Index" }
            );

            context.MapRoute(
                "TaiMuiHongsIndex",
                "Admin/Benh-Tai-Mui-Hong",
                new { controller = "TaiMuiHongs", action = "Index" }
            );

            context.MapRoute(
                "RangHamMatsIndex",
                "Admin/Benh-Rang-Ham-Mat",
                new { controller = "RangHamMats", action = "Index" }
            );

            context.MapRoute(
                "DaLieuxIndex",
                "Admin/Benh-Da-Lieu",
                new { controller = "DaLieux", action = "Index" }
            );

            context.MapRoute(
                "NationsIndex",
                "Admin/Danh-Muc-Quoc-Gia",
                new { controller = "Nations", action = "Index" }
            );

            context.MapRoute(
                "Nation1Index",
                "Admin/Danh-Muc-Dan-Toc",
                new { controller = "Nation1", action = "Index" }
            );

            context.MapRoute(
                "WardsIndex",
                "Admin/Danh-Muc-Phuong-Xa",
                new { controller = "Wards", action = "Index" }
            );

            context.MapRoute(
                "DistrictsIndex",
                "Admin/Danh-Muc-Quan-Huyen",
                new { controller = "Districts", action = "Index" }
            );

            context.MapRoute(
                "HomeTownsIndex",
                "Admin/Danh-Muc-Tinh-Thanh",
                new { controller = "HomeTowns", action = "Index" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}