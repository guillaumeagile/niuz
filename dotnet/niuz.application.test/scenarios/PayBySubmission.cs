using FluentAssertions;
using niuz.application.finance;
using niuz.application.fixtures;
using niuz.application.hr;
using niuz.application.website;
using Xunit;

namespace niuz.application.scenarios
{
    public class PayBySubmission
    {
        private readonly TestFacade app;

        public PayBySubmission()
        {
            app = new TestFacade();
        }

        [Fact]
        public void SubmitArticle()
        {
            app.Dispatch(new HireAuthor("author-1", "Freddy Kruger", "pay-by-submission", 50, "123-4567-89"));
            app.Submit("article-1", "author-1", "headline");

            app.Get("homepage").Should().BeEmpty();
            app.GetByBankAccount("123-4567-89").Should().ContainInOrder(
                new PaymentDto(50, "123-4567-89", "Freddy Kruger", "headline")
            );
        }

        [Fact]
        public void PublishArticle()
        {
            app.Dispatch(new HireAuthor("author-1", "Freddy Kruger", "pay-by-submission", 50, "123-4567-89"));
            app.Submit("article-1", "author-1", "headline");
            app.Publish("article-1");

            app.Get("homepage").Should().ContainInOrder(
                new TeaserDto("headline", "Freddy Kruger")
            );
            app.GetByBankAccount("123-4567-89").Should().ContainInOrder(
                new PaymentDto(50, "123-4567-89", "Freddy Kruger", "headline")
            );
        }
    }
}