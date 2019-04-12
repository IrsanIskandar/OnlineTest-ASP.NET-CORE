using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfinetworksOnlineTest.Models
{
    public class AnswerModel
    {
        public long ID { get; set; }
        [Required]
        public string AnswersOne { get; set; }
        [Required]
        public string AnswersTwo { get; set; }
        [Required]
        public string AnswersThree { get; set; }
        [Required]
        public string AnswersFourth { get; set; }
        [Required]
        public string AnswersFive { get; set; }
        [Required]
        public string AnswersSix { get; set; }
        [Required]
        public string AnswersSeven { get; set; }
        [Required]
        public string AnswersEight { get; set; }
        [Required]
        public string AnswersNine { get; set; }
        public DateTime DateAsnwers { get; set; }
        public long Usr_InterviewID { get; set; }
    }
}
