using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AРМ_Библиотекаря.Classes
{
    public class Variables
    {
        public class Reader
        {
            public string ReaderNumber { get; set; }
            public string ReaderSNP { get; set; }
            public string ReaderPhone { get; set; }
            public string ReaderAddress { get; set; }
            public string ReaderEducation { get; set; }
            public string ReaderBithdate { get; set; }
            public string ReaderPlaceOfStudy { get; set; }
            public string ReaderClass { get; set; }
            public string ReaderAgeLimit { get; set; }
            
        }

        public class Book
        {
            public string BookNumber { get; set; }
            public string BookName { get; set; }
            public string BookAutor { get; set; }
            public string BookGenre { get; set; }
            public string BookYear { get; set; }
            public string BookPublishing { get; set; }
            public string BookPages { get; set; }
            public string BookAge { get; set; }
            public int BookGenreIds { get; set; }
            public string IsEnable { get; set; }
        }

        public class Genres
        { 
            public int GenreId { get; set; }
            public string GenreName { get; set; }
            public bool IsChecked { get; set; }
        }
        public class Authors
        {
            public int AuthorId { get; set; }
            public string AuthorName { get; set; }
            public bool IsChecked { get; set; }
        }
        public class Publishing
        {
            public int PublishingId { get; set; }
            public string PublishingName { get; set; }         
        }
        public class City
        {
            public int CityId { get; set; }
            public string CityName { get; set; }
        }

        public class AddBooks
        {        
            public string BookNumber { get; set; }
            public string BookName { get; set; }
            public string BookAutor { get; set; }
            public string BookGenre { get; set; }
            public string BookYear { get; set; }
            public string BookPublishing { get; set; }
            public string BookPages { get; set; }
            public string BookAge { get; set; }
            public string BookDateAccompanying { get; set; }
            public string BookNumberAccompanying { get; set; }
            public Visibility IsDeleteButtonVisible { get; set; }
        }

        public class IssueBook
        { 
            public string NumberReaderCard { get; set; }
            public string ReaderName { get; set; }           
            public string BookName { get; set; }
            public string BookNumber { get; set; }
            public string DataIssue { get; set; }
            public string DataPass { get; set; }
            public string Condition { get; set; }
            
        }
    }
}
