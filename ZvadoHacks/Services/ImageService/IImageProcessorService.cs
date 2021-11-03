using System.Collections.Generic;
using System.Threading.Tasks;
using ZvadoHacks.Models;

namespace ZvadoHacks.Services.ImageService
{
    public interface IImageProcessorService
    {
        Task Process(IEnumerable<ImageInputModel> images);

        Task Process(ImageInputModel image);

    }
}
