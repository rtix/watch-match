using System.ComponentModel.DataAnnotations;

namespace WatchMatchApi.Models
{
    public class Room
    {
        private string? _guestId;

        [RegularExpression("^[a-zA-Z0-9]{4}$", ErrorMessage = "Invalid room id.")]
        public required string Id { get; init; }

        [MinLength(1, ErrorMessage = "Creator ID cannot be empty.")]
        public required string CreatorId { get; init; }

        [MinLength(1, ErrorMessage = "Guest ID cannot be empty.")]
        public string? GuestId
        {
            get => _guestId;
            set 
            {
                if (_guestId != null)
                {
                    throw new InvalidOperationException("Guest ID is already defined.");
                }
                _guestId = value;
            }
        }

        public bool IsFull => CreatorId != null && GuestId != null;
        public override string ToString() => $"{Id} : {CreatorId} : {GuestId}";
    }
}
