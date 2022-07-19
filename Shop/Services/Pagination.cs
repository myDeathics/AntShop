namespace Shop.Services
{
    public class Pagination
    {
        private int ElementsInPage;
        private int CountOfElements;

        public Pagination(int CountOfElements, int ElementsInPage = 10)
        {
            this.ElementsInPage = ElementsInPage;
            this.CountOfElements = CountOfElements;
        }
        public int GetElements(int pageIndex)
        {
            if (CountOfElements - pageIndex * ElementsInPage < ElementsInPage && ElementsInPage * 2 <= CountOfElements) return CountOfElements - pageIndex * ElementsInPage;
            return ElementsInPage;
        }
        public int GetElementsWhichGot(int pageIndex)
        {
            return ElementsInPage * (pageIndex - 1);
        }
    }

}
