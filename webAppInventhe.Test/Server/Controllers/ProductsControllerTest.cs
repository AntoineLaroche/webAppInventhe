using Xunit;
using System.Threading.Tasks;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using webAppInventhe.Server.Controllers;
using webAppInventhe.Shared;
using webAppInventhe.Shared.Model;
using KellermanSoftware.CompareNetObjects;
using System.Linq;
using webAppInventhe.Shared.Exception;

namespace webAppInventhe.Test.Server.Controllers
{
    public class ProductsControllerTest
    {
        private readonly IFixture _fixture;
        private readonly ProductsController _sut;
        private readonly Mock<IRepository> _mockRepository;
        private readonly CompareLogic _compareLogic;

        public ProductsControllerTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockRepository = _fixture.Freeze<Mock<IRepository>>();
            _compareLogic = new CompareLogic();
            _sut = _fixture.Create<ProductsController>();   
        }

        [Fact]
        public async Task GivenValidModelState_GetProducts_ShouldReturnExpectedProducts()
        {
            var expectedProductsList = _fixture.CreateMany<Products>().ToList();
            _mockRepository.Setup(m => m.GetProducts()).ReturnsAsync(expectedProductsList);

            var result = await _sut.GetProducts();

            var comparisonResult = _compareLogic.Compare(expectedProductsList, result);
            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public async Task GivenValidModelState_PutProducts_ShouldUpdateProducts()
        {
            var produtcs = _fixture.Create<Products>();

            await _sut.PutProducts(produtcs);

            _mockRepository.Verify(m => m.UpdateProduct(It.Is<Products>( p => p.Name == produtcs.Name)),Times.Once);
        }

        [Fact]
        public async Task GivenNotValidModelState_PutProducts_ShouldThrowBadRequestException()
        {
            _sut.ModelState.AddModelError("Name", "Required");
            await Assert.ThrowsAsync<BadRequestException>(async () => await _sut.PutProducts(_fixture.Create<Products>()));
        }

        [Fact]
        public async Task GivenValidModelState_CreateProducts_ShouldUpdateProducts()
        {
            var produtcs = _fixture.Create<Products>();

            await _sut.CreateProducts(produtcs);

            _mockRepository.Verify(m => m.UpdateProduct(It.Is<Products>(p => p.Name == produtcs.Name)), Times.Once);
        }

        [Fact]
        public async Task GivenNotValidModelState_CreateProducts_ShouldThrowBadRequestException()
        {
            _sut.ModelState.AddModelError("Name", "Required");
            await Assert.ThrowsAsync<BadRequestException>(async () => await _sut.CreateProducts(_fixture.Create<Products>()));
        }

        [Fact]
        public async Task GivenValidModelState_DeleteProducts_ShouldDeleteProducts()
        {
            var produtcs = _fixture.Create<Products>();

            await _sut.DeleteProducts(produtcs.Name);

            _mockRepository.Verify(m => m.DeleteProduct(It.Is<string>(s => s == produtcs.Name)), Times.Once);
        }

        [Fact]
        public async Task GivenNotValidModelState_DeleteProducts_ShouldThrowBadRequestException()
        {
            _sut.ModelState.AddModelError("Name", "Required");
            await Assert.ThrowsAsync<BadRequestException>(async () => await _sut.DeleteProducts(_fixture.Create<string>()));
        }
    }
}
