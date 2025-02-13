using System;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace MediaLibrary
{
    public partial class MainWindow : Window
    {
        private MediaContext _dbContext;

        public MainWindow()
        {
            InitializeComponent();
            _dbContext = new MediaContext();
            _dbContext.Database.EnsureCreated();
            LoadMedia();
        }

        private void LoadMedia()
        {
            MediaList.ItemsSource = _dbContext.Media
                .Select(m => new Media
                {
                    ID = m.ID,
                    Title = m.Title,
                    Author = m.Author,
                    Genre = m.Genre,
                    Type = m.Type,
                    IsBorrowed = m.IsBorrowed
                })
                .ToList();
        }

        private void AddMedia_Click(object sender, RoutedEventArgs e)
        {
            if (TypeInput.SelectedItem is ComboBoxItem selectedType)
            {
                var newMedia = new Media
                {
                    Title = TitleInput.Text,
                    Author = AuthorInput.Text,
                    Genre = GenreInput.Text,
                    Type = selectedType.Content.ToString(),
                    IsBorrowed = false
                };

                _dbContext.Media.Add(newMedia);
                _dbContext.SaveChanges();
                LoadMedia();
            }
            else
            {
                MessageBox.Show("Wybierz typ mediów!");
            }
        }

        private void DeleteMedia_Click(object sender, RoutedEventArgs e)
        {
            var selectedMedia = MediaList.SelectedItem as Media;
            if (selectedMedia != null)
            {
                var mediaToDelete = _dbContext.Media.FirstOrDefault(m => m.ID == selectedMedia.ID);
                if (mediaToDelete != null)
                {
                    _dbContext.Media.Remove(mediaToDelete);
                    _dbContext.SaveChanges();
                    LoadMedia();
                }
            }
        }

        private void MarkBorrowed_Click(object sender, RoutedEventArgs e)
        {
            var selectedMedia = MediaList.SelectedItem as Media;
            if (selectedMedia != null)
            {
                var mediaToMark = _dbContext.Media.FirstOrDefault(m => m.ID == selectedMedia.ID);
                if (mediaToMark != null)
                {
                    mediaToMark.IsBorrowed = !mediaToMark.IsBorrowed;
                    _dbContext.SaveChanges();
                    LoadMedia();
                }
            }
        }

        private void FilterMedia(object sender, RoutedEventArgs e)
        {
            var filter = AuthorFilter.Text;
            MediaList.ItemsSource = _dbContext.Media
                .Where(m => m.Author.Contains(filter))
                .Select(m => new Media
                {
                    ID = m.ID,
                    Title = m.Title,
                    Author = m.Author,
                    Genre = m.Genre,
                    Type = m.Type,
                    IsBorrowed = m.IsBorrowed
                })
                .ToList();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var mediaList = _dbContext.Media.ToList();
            var reportPath = "MediaReport.txt";
            using (var writer = new StreamWriter(reportPath))
            {
                writer.WriteLine("Raport Biblioteki Mediów");
                writer.WriteLine("========================");
                foreach (var media in mediaList)
                {
                    writer.WriteLine($"Tytuł: {media.Title}, Autor: {media.Author}, Gatunek: {media.Genre}, Typ: {media.Type}, Wypożyczone: {(media.IsBorrowed ? "Tak" : "Nie")}");
                }
            }

            MessageBox.Show($"Raport został zapisany w pliku: {Path.GetFullPath(reportPath)}");
        }

        private void MediaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obsługa zmiany wyboru, jeśli potrzebna
        }
    }

    public class MediaContext : DbContext
    {
        public DbSet<Media> Media { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=media.db");
        }
    }

    public class Media
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Type { get; set; }
        public bool IsBorrowed { get; set; }
    }
}
