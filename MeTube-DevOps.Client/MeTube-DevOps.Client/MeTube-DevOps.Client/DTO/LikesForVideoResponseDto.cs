using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTube_DevOps.Client.DTO
{
    public class LikesForVideoResponseDto
    {
        public int Count { get; set; }
        public IEnumerable<LikeDto> Likes { get; set; }
    }
}
