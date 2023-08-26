using WebApplication1.Controllers;

namespace TestProject1
{
    public class WeatherForecastControllerTest
    {
        [Fact]
        public void DeveFazerGetComSucesso()
        {
            var controler = new WeatherForecastController();

            var result = controler.Get();

            Assert.True(result.Count() >  0);
        } 
    }
}  