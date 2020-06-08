using Nats.Setvice.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Nats.Client.Domain.Model
{
    [Serializable]
    [Table("messageForSave")]
    public class MessageForSave : Entity
    {
        [Required]
        public int Number { get; set; }

        [Required(ErrorMessage = "Text dont be empty")]

        public string Text { get; set; }

        [Required]
        public DateTime TimeSend { get; set; }

        [Required]
        public int HashCode { get; set; }
    }
}
