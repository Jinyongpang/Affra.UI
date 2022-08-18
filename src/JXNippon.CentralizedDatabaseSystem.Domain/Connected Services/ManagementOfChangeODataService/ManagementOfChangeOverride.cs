using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using Microsoft.OData.Client;

namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.ManagementOfChanges
{
    public partial class ManagementOfChangeRecord
    {
        [IgnoreClientProperty]
        public DateTime CreateDateTimeUI
        {
            get { return this.CreatedDateTime.ToLocalDateTime(); }
            set { this.CreatedDateTime = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Identifications
{
    public partial class Identification
    {
        [IgnoreClientProperty]
        public DateTime ExpiryDateUI
        {
            get { return this.ExpiryDate.ToLocalDateTime(); }
            set { this.ExpiryDate = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.AuthorisationAndApprovals
{
    public partial class AuthorisationAndApproval
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.CloseOuts
{
    public partial class CloseOut
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.CommunicationAndImplementations
{
    public partial class CommunicationAndImplementation
    {
        [IgnoreClientProperty]
        public DateTime AgreedByDateUI
        {
            get { return this.AgreedByDate.ToLocalDateTime(); }
            set { this.AgreedByDate = value.ToUniversalTime(); }
        }

        [IgnoreClientProperty]
        public DateTime PreparedByDateUI
        {
            get { return this.PreparedByDate.ToLocalDateTime(); }
            set { this.PreparedByDate = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Endorsements
{
    public partial class Endorsement
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace ManagementOfChangeODataService.Affra.Service.ManagementOfChange.Domain.Extensions
{
    public partial class Extension
    {
        [IgnoreClientProperty]
        public DateTime ReviewDateUI
        {
            get { return this.ReviewDate.ToLocalDateTime(); }
            set { this.ReviewDate = value.ToUniversalTime(); }
        }
    }
}
