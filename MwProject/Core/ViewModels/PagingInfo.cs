using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Model Widoku do przekazywania danych między kontrolerem a widokiem
// aby zapewnić prawidłowe działanie atrybutów pomocniczych znaczników 
// Przechowujemy w nim informację:
// o ogólnej liczbie pozycji
// o wyświetlanej ilości pozycji na stronie
// o bieżącej stronie
// oraz o ilości stron Wyliczamy dzieląc ogólną liczbę pozycji przez ilość pozycji na stronie

namespace MwProject.Core.ViewModels
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
