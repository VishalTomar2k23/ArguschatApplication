namespace ChatApp.Business.Helpers
{
    public static class ClaimsConstant
    { 
        // we need to create a constant file for basic details
        // we will have first name , last name , image name and destination 
        public const string FirstNameClaim = "firstName";
        public const string LastNameClaim = "lastName";
        public const string ImagePathClaim = "imagePath";
        public const string DesignationClaim = "designation";
       

    }

    // we will have two types of profiles -> admin and user 

    public enum ProfileType
    {
        User = 1,
        Administrator = 2
    }
}
