namespace Football_League.Common
{
    public static class ErrorMessages
    {
        public const string EntityNotFound = "Entity {0} with Id {1} was not found.";
        public const string TeamAlreadyExists = "A team with name {0} already exists.";
        public const string MatchSameTeams = "Home Team and Away Team must be different.";
        public const string InvalidSortingType = "The Sorting Type {0} is invalid.";
    }
}
