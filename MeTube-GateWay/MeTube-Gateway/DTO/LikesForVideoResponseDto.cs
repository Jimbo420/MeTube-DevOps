using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTube_GateWay.DTO
{
    public class LikesForVideoResponseDto
    {
        public int Count { get; set; }
        public IEnumerable<LikeDto> Likes { get; set; }
    }
}
