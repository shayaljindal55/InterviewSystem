using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewSystem.Models
{

    public class CandidateResult
    {
        public CandidateResult()
        {
            InterviewRounds = new List<RoundDetails>();
        }

        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AverageRating { get; set; }
        public int Rank { get; set; }
        public string Result { get; set; }
        public List<RoundDetails> InterviewRounds { get; set; }

    }

    public class RoundDetails
    {
        public string RoundName { get; set; }
        public int TotalRating { get; set; }
        public int TotalRound { get; set; }
    }


    public class RankList
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Rank { get; set; }
    }
}