using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Data;
public class ClientDb : DbContext
{
    public ClientDb(DbContextOptions<ClientDb> options)
        : base(options) { 
            ClientInfos = Set<ClientInfo>();
        }

    public DbSet<ClientInfo> ClientInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ClientInfo>().HasKey(c => c.Id);
        modelBuilder.Entity<ClientInfo>().Property(c => c.Id).ValueGeneratedOnAdd();
        SeedData(modelBuilder);
    }

    public void SeedData(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ClientInfo>().HasData(
            new ClientInfo { Id = 1, FirstName = "John", LastName = "Smith", Email = "John.Smith@email.com" },
            new ClientInfo { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "Jane.Doe@email.com" },
            new ClientInfo { Id = 3, FirstName = "Bill", LastName = "Gates", Email = "Bill.Gates@email.com" },
            new ClientInfo { Id = 4, FirstName = "Oprah", LastName = "Winfrey", Email = "Oprah.Winfrey@email.com" },
            new ClientInfo { Id = 5, FirstName = "Barack", LastName = "Obama", Email = "Barack.Obama@email.com" },
            new ClientInfo { Id = 6, FirstName = "George", LastName = "Washington", Email = "George.Washington@email.com" },
            new ClientInfo { Id = 7, FirstName = "Thomas", LastName = "Jefferson", Email = "Thomas.Jefferson@email.com" },
            new ClientInfo { Id = 8, FirstName = "Benjamin", LastName = "Franklin", Email = "Benjamin.Franklin@email.com" },
            new ClientInfo { Id = 9, FirstName = "Abraham", LastName = "Lincoln", Email = "Abraham.Lincoln@email.com" },
            new ClientInfo { Id = 10, FirstName = "Teddy", LastName = "Roosevelt", Email = "Teddy.Roosevelt@email.com" },
            new ClientInfo { Id = 11, FirstName = "John", LastName = "Kennedy", Email = "John.Kennedy@email.com" },
            new ClientInfo { Id = 12, FirstName = "Richard", LastName = "Nixon", Email = "Richard.Nixon@email.com" },
            new ClientInfo { Id = 13, FirstName = "Gerald", LastName = "Ford", Email = "Gerald.Ford@email.com" },
            new ClientInfo { Id = 14, FirstName = "Jimmy", LastName = "Carter", Email = "Jimmy.Carter@email.com" },
            new ClientInfo { Id = 15, FirstName = "Ronald", LastName = "Reagan", Email = "Ronald.Reagan@email.com" },
            new ClientInfo { Id = 16, FirstName = "George", LastName = "Bush", Email = "George.Bush@email.com" },
            new ClientInfo { Id = 17, FirstName = "Bill", LastName = "Clinton", Email = "Bill.Clinton@email.com" },
            new ClientInfo { Id = 18, FirstName = "George", LastName = "Bush", Email = "George.Bush@email.com" },
            new ClientInfo { Id = 19, FirstName = "Barack", LastName = "Obama", Email = "Barack.Obama@email.com" },
            new ClientInfo { Id = 20, FirstName = "Donald", LastName = "Trump", Email = "Donald.Trump@email.com" },
            new ClientInfo { Id = 21, FirstName = "Paul", LastName = "McCartney", Email = "Paul.McCartney@email.com" },
            new ClientInfo { Id = 22, FirstName = "John", LastName = "Lennon", Email = "John.Lennon@email.com" },
            new ClientInfo { Id = 23, FirstName = "George", LastName = "Harrison", Email = "George.Harrison@email.com" },
            new ClientInfo { Id = 24, FirstName = "Ringo", LastName = "Starr", Email = "Ringo.Starr@email.com" },
            new ClientInfo { Id = 25, FirstName = "Elvis", LastName = "Presley", Email = "Elvis.Presley@email.com" },
            new ClientInfo { Id = 26, FirstName = "Madonna", LastName = "Ciccone", Email = "Madonna.Ciccone@email.com" },
            new ClientInfo { Id = 27, FirstName = "Michael", LastName = "Jackson", Email = "Michael.Jackson@email.com" },
            new ClientInfo { Id = 28, FirstName = "Prince", LastName = "Rogers", Email = "Prince.Rogers@email.com" },
            new ClientInfo { Id = 29, FirstName = "Bruce", LastName = "Springsteen", Email = "Bruce.Springsteen@email.com" },
            new ClientInfo { Id = 30, FirstName = "Beyonce", LastName = "Knowles", Email = "Beyonce.Knowles@email.com" },
            new ClientInfo { Id = 31, FirstName = "Jay-Z", LastName = "Carter", Email = "Jay-Z.Carter@email.com" },
            new ClientInfo { Id = 32, FirstName = "Taylor", LastName = "Swift", Email = "Taylor.Swift@email.com" },
            new ClientInfo { Id = 33, FirstName = "Lady", LastName = "Gaga", Email = "Lady.Gaga@email.com" },
            new ClientInfo { Id = 34, FirstName = "Adele", LastName = "Adkins", Email = "Adele.Adkins@email.com" },
            new ClientInfo { Id = 35, FirstName = "Kendrick", LastName = "Lamar", Email = "Kendrick.Lamar@email.com" },
            new ClientInfo { Id = 36, FirstName = "Drake", LastName = "Graham", Email = "Drake.Graham@email.com" },
            new ClientInfo { Id = 37, FirstName = "Harry", LastName = "Styles", Email = "Harry.Styles@email.com" },
            new ClientInfo { Id = 38, FirstName = "Taylor", LastName = "Swift", Email = "Taylor.Swift@email.com" },
            new ClientInfo { Id = 39, FirstName = "Lionel", LastName = "Richie", Email = "Lionel.Richie@email.com" },
            new ClientInfo { Id = 40, FirstName = "Stevie", LastName = "Wonder", Email = "Stevie.Wonder@email.com" },
            new ClientInfo { Id = 41, FirstName = "Marvin", LastName = "Gaye", Email = "Marvin.Gaye@email.com" },
            new ClientInfo { Id = 42, FirstName = "Diana", LastName = "Ross", Email = "Diana.Ross@email.com" },
            new ClientInfo { Id = 43, FirstName = "Aretha", LastName = "Franklin", Email = "Aretha.Franklin@email.com" },
            new ClientInfo { Id = 44, FirstName = "James", LastName = "Brown", Email = "James.Brown@email.com" },
            new ClientInfo { Id = 45, FirstName = "Ray", LastName = "Charles", Email = "Ray.Charles@email.com" },
            new ClientInfo { Id = 46, FirstName = "Nat", LastName = "King", Email = "Nat.King@email.com" },
            new ClientInfo { Id = 47, FirstName = "Tony", LastName = "Bennett", Email = "Tony.Bennett@email.com" },
            new ClientInfo { Id = 48, FirstName = "Cher", LastName = "Sonny", Email = "Cher.Sonny@email.com" },
            new ClientInfo { Id = 49, FirstName = "Bob", LastName = "Dylan", Email = "Bob.Dylan@email.com" },
            new ClientInfo { Id = 50, FirstName = "Barbra", LastName = "Streisand", Email = "Barbra.Streisand@email.com" },
            new ClientInfo { Id = 51, FirstName = "Neil", LastName = "Young", Email = "Neil.Young@email.com" },
            new ClientInfo { Id = 52, FirstName = "Joni", LastName = "Mitchell", Email = "Joni.Mitchell@email.com" },
            new ClientInfo { Id = 53, FirstName = "Paul", LastName = "Simon", Email = "Paul.Simon@email.com" },
            new ClientInfo { Id = 54, FirstName = "Elton", LastName = "John", Email = "Elton.John@email.com" },
            new ClientInfo { Id = 55, FirstName = "David", LastName = "Bowie", Email = "David.Bowie@email.com" },
            new ClientInfo { Id = 56, FirstName = "Freddie", LastName = "Mercury", Email = "Freddie.Mercury@email.com" },
            new ClientInfo { Id = 57, FirstName = "Queen", LastName = "band", Email = "Queen.band@email.com" },
            new ClientInfo { Id = 58, FirstName = "The", LastName = "Beatles", Email = "The.Beatles@email.com" },
            new ClientInfo { Id = 59, FirstName = "Rolling", LastName = "Stones", Email = "Rolling.Stones@email.com" },
            new ClientInfo { Id = 60, FirstName = "Led", LastName = "Zeppelin", Email = "Led.Zeppelin@email.com" },
            new ClientInfo { Id = 61, FirstName = "Jimi", LastName = "Hendrix", Email = "Jimi.Hendrix@email.com" },
            new ClientInfo { Id = 62, FirstName = "Janis", LastName = "Joplin", Email = "Janis.Joplin@email.com" },
            new ClientInfo { Id = 63, FirstName = "The", LastName = "Who", Email = "The.Who@email.com" },
            new ClientInfo { Id = 64, FirstName = "The", LastName = "Doors", Email = "The.Doors@email.com" },
            new ClientInfo { Id = 65, FirstName = "Pink", LastName = "Floyd", Email = "Pink.Floyd@email.com" },
            new ClientInfo { Id = 66, FirstName = "Grateful", LastName = "Dead", Email = "Grateful.Dead@email.com" },
            new ClientInfo { Id = 67, FirstName = "Chuck", LastName = "Berry", Email = "Chuck.Berry@email.com" },
            new ClientInfo { Id = 68, FirstName = "Little", LastName = "Richard", Email = "Little.Richard@email.com" },
            new ClientInfo { Id = 69, FirstName = "Fats", LastName = "Domino", Email = "Fats.Domino@email.com" },
            new ClientInfo { Id = 70, FirstName = "Jerry", LastName = "Lee", Email = "Jerry.Lee@email.com" },
            new ClientInfo { Id = 71, FirstName = "Ike", LastName = "Tina", Email = "Ike.Tina@email.com" },
            new ClientInfo { Id = 72, FirstName = "The", LastName = "Temptations", Email = "The.Temptations@email.com" },
            new ClientInfo { Id = 73, FirstName = "Diana", LastName = "Ross", Email = "Diana.Ross@email.com" },
            new ClientInfo { Id = 74, FirstName = "Smokey", LastName = "Robinson", Email = "Smokey.Robinson@email.com" },
            new ClientInfo { Id = 75, FirstName = "Marvin", LastName = "Gaye", Email = "Marvin.Gaye@email.com" },
            new ClientInfo { Id = 76, FirstName = "Stevie", LastName = "Wonder", Email = "Stevie.Wonder@email.com" },
            new ClientInfo { Id = 77, FirstName = "Supremes", LastName = "band", Email = "Supremes.band@email.com" },
            new ClientInfo { Id = 78, FirstName = "Bob", LastName = "Dylan", Email = "Bob.Dylan@email.com" },
            new ClientInfo { Id = 79, FirstName = "Rolling", LastName = "Stones", Email = "Rolling.Stones@email.com" },
            new ClientInfo { Id = 80, FirstName = "Led", LastName = "Zeppelin", Email = "Led.Zeppelin@email.com" },
            new ClientInfo { Id = 81, FirstName = "The", LastName = "Beatles", Email = "The.Beatles@email.com" },
            new ClientInfo { Id = 82, FirstName = "Jimi", LastName = "Hendrix", Email = "Jimi.Hendrix@email.com" },
            new ClientInfo { Id = 83, FirstName = "Janis", LastName = "Joplin", Email = "Janis.Joplin@email.com" },
            new ClientInfo { Id = 84, FirstName = "The", LastName = "Who", Email = "The.Who@email.com" },
            new ClientInfo { Id = 85, FirstName = "The", LastName = "Doors", Email = "The.Doors@email.com" },
            new ClientInfo { Id = 86, FirstName = "Pink", LastName = "Floyd", Email = "Pink.Floyd@email.com" },
            new ClientInfo { Id = 87, FirstName = "Grateful", LastName = "Dead", Email = "Grateful.Dead@email.com" },
            new ClientInfo { Id = 88, FirstName = "The", LastName = "Beach", Email = "The.Beach@email.com" },
            new ClientInfo { Id = 89, FirstName = "Boys", LastName = "band", Email = "Boys.band@email.com" },
            new ClientInfo { Id = 90, FirstName = "Bruce", LastName = "Springsteen", Email = "Bruce.Springsteen@email.com" },
            new ClientInfo { Id = 91, FirstName = "Nirvana", LastName = "band", Email = "Nirvana.band@email.com" },
            new ClientInfo { Id = 92, FirstName = "Pearl", LastName = "Jam", Email = "Pearl.Jam@email.com" },
            new ClientInfo { Id = 93, FirstName = "Red", LastName = "Hot", Email = "Red.Hot@email.com" },
            new ClientInfo { Id = 94, FirstName = "Chili", LastName = "Peppers", Email = "Chili.Peppers@email.com" },
            new ClientInfo { Id = 95, FirstName = "Foo", LastName = "Fighters", Email = "Foo.Fighters@email.com" },
            new ClientInfo { Id = 96, FirstName = "Radiohead", LastName = "band", Email = "Radiohead.band@email.com" },
            new ClientInfo { Id = 97, FirstName = "Green", LastName = "Day", Email = "Green.Day@email.com" },
            new ClientInfo { Id = 98, FirstName = "U2", LastName = "band", Email = "U2.band@email.com" },
            new ClientInfo { Id = 99, FirstName = "Coldplay", LastName = "band", Email = "Coldplay.band@email.com" },
            new ClientInfo { Id = 100, FirstName = "Ed", LastName = "Sheeran", Email = "Ed.Sheeran@email.com" });
    }
}