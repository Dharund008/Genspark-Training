using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Twitter.Models
{
    public class Follower
    {
        public int Id { get; set; }
        public int FollowerId { get; set; }//one who follows
        public int FollowingId { get; set; }//one who is followed

        [ForeignKey("FollowerId")]
        public User? FollowUser { get; set; }

        [ForeignKey("FollowingId")]
        public User? FollowingUser { get; set; }
    }
}