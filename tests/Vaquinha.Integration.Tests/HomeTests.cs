using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Vaquinha.Domain.Extensions;
using Vaquinha.Domain.ViewModels;
using Vaquinha.Integration.Tests.Fixtures;
using Vaquinha.MVC;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.Integration.Tests
{
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]


    public class HomeTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _integrationTestsFixture;
        private readonly DoacaoFixture _doacaoFixture;
        private readonly EnderecoFixture _enderecoFixture;
        private readonly CartaoCreditoFixture _cartaoCreditoFixture;
        public HomeTests(IntegrationTestsFixture<StartupWebTests> integrationTestsFixture)
        {
            _integrationTestsFixture = integrationTestsFixture;
            _doacaoFixture = new DoacaoFixture();
            _enderecoFixture = new EnderecoFixture();
            _cartaoCreditoFixture = new CartaoCreditoFixture();
        }

        [Trait("HomeControllerIntegrationTests", "HomeController_CarregarPaginaInicial_TotalDoadoresETotalValorArrecadadoDeveSerZero")]
        [Fact]
        public async Task HomeController_CarregarPaginaInicial_TotalDoadoresETotalValorArrecadadoDeveSerZero()
        {
            // Arrange & Act
            var home = await _integrationTestsFixture.Client.GetAsync("Home");

            // Assert
            home.EnsureSuccessStatusCode();
            var dadosHome = await home.Content.ReadAsStringAsync();

            var totalArrecadado = 0.ToDinheiroBrString();
            var metaCampanha = _integrationTestsFixture.ConfiguracaoGeralAplicacao.MetaCampanha.ToDinheiroBrString();

            // Dados totais da doação
            dadosHome.Should().Contain(expected: "Arrecadamos quanto?");
            dadosHome.Should().Contain(expected: totalArrecadado);

            dadosHome.Should().Contain(expected: "Quanto falta arrecadar?");
            dadosHome.Should().Contain(expected: metaCampanha);
        }

    }
}
      