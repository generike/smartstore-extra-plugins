using System.Data.Entity.ModelConfiguration;

namespace SmartStore.MailChimp.Data
{
    public class MailChimpEventQueueRecordMap : EntityTypeConfiguration<MailChimpEventQueueRecord>
    {
        public MailChimpEventQueueRecordMap()
        {
            ToTable("MailChimpEventQueueRecord");

            HasKey(m => m.Id);
            Property(m => m.Email).IsRequired().HasMaxLength(255);
            Property(m => m.IsSubscribe).IsRequired();
        }
    }
}