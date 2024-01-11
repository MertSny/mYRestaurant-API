using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constansts
{
    public static class Messages
    {
        public readonly static string AuthorizationDenied = "Yetkiniz bulunmamaktadır.";
        public readonly static string UpdatedFailure = "Güncelleme hatası oluştu";
        public readonly static string UpdatedSuccess = "Güncelleme başarılı";
        public readonly static string CreatedFailure = "Oluşturma hatası oluştu";
        public readonly static string CreatedSuccess = "Oluşturma başarılı";
        public readonly static string DeletedFailure = "Silme hatası oluştu";
        public readonly static string DeletedSuccess = "Silme başarılı";
        public readonly static string NotFound = "Kayıt bulunamadı";
        public readonly static string FieldNotNull = "Lütfen boş bırakmayınız";
    }
}
