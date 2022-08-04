using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Entities;
using ShoppingCart.Enums;
using ShoppingCart.Helpers;

namespace ShoppingCart.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;

        public SeedDb(DataContext context, IUserHelper userHelper,IImageHelper imageHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
            await CheckProductsAsync();
            await CheckRolesAsync();
            await CheckUserAsync("0801-1987-13256", "Gerardo", "Lanza", "gerardo@yopmail.com", "3307-7964", "Tegucigalpa MDC",
            UserType.Admin);

        }


        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Adidas Barracuda", 270000M, 12F, new List<string>() { "Calzado", "Deportes" }, new
                List<string>() { "adidas_barracuda.png" });
                await AddProductAsync("Adidas Superstar", 250000M, 12F, new List<string>() { "Calzado", "Deportes" }, new
                List<string>() { "Adidas_superstar.png" });
                await AddProductAsync("AirPods", 1300000M, 12F, new List<string>() { "Tecnología", "Apple" }, new List<string>() {"airpos.png", "airpos2.png" });
                await AddProductAsync("Audifonos Bose", 870000M, 12F, new List<string>() { "Tecnología" }, new List<string>() {"audifonos_bose.png" });
                await AddProductAsync("Bicicleta Ribble", 12000000M, 6F, new List<string>() { "Deportes" }, new List<string>() {"bicicleta_ribble.png" });
                await AddProductAsync("Camisa Cuadros", 56000M, 24F, new List<string>() { "Ropa" }, new List<string>() {"camisa_cuadros.png" });
                await AddProductAsync("Casco Bicicleta", 820000M, 12F, new List<string>() { "Deportes" }, new List<string>() {"casco_bicicleta.png", "casco.png" });
                await AddProductAsync("iPad", 2300000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() {"ipad.png" });
                await AddProductAsync("iPhone 13", 5200000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>(){ "iphone13.png", "iphone13b.png", "iphone13c.png", "iphone13d.png" });
                await AddProductAsync("Mac Book Pro", 12100000M, 6F, new List<string>() { "Tecnología", "Apple" }, new
                List<string>() { "mac_book_pro.png" });
                await AddProductAsync("Mancuernas", 370000M, 12F, new List<string>() { "Deportes" }, new List<string>() {"mancuernas.png" });
                await AddProductAsync("Mascarilla Cara", 26000M, 100F, new List<string>() { "Belleza" }, new List<string>() {"mascarilla_cara.png" });
                await AddProductAsync("New Balance 530", 180000M, 12F, new List<string>() { "Calzado", "Deportes" }, new
                List<string>() { "newbalance530.png" });
                await AddProductAsync("New Balance 565", 179000M, 12F, new List<string>() { "Calzado", "Deportes" }, new
                List<string>() { "newbalance565.png" });
                await AddProductAsync("Nike Air", 233000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() {"nike_air.png" });
                await AddProductAsync("Nike Zoom", 249900M, 12F, new List<string>() { "Calzado", "Deportes" }, new
                List<string>() { "nike_zoom.png" });
                await AddProductAsync("Buso Adidas Mujer", 134000M, 12F, new List<string>() { "Ropa", "Deportes" }, new
                List<string>() { "buso_adidas.png" });
                await AddProductAsync("Suplemento Boots Original", 15600M, 12F, new List<string>() { "Nutrición" }, new
                List<string>() { "Boost_Original.png" });
                
                await AddProductAsync("Whey Protein", 252000M, 12F, new List<string>() { "Nutrición" }, new List<string>() {"whey_protein.png" });
                await AddProductAsync("Arnes Mascota", 25000M, 12F, new List<string>() { "Mascotas" }, new List<string>() {"arnes_mascota.png" });
                await AddProductAsync("Cama Mascota", 99000M, 12F, new List<string>() { "Mascotas" }, new List<string>() {"cama_mascota.png" });
                await AddProductAsync("Teclado Gamer", 67000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new
                List<string>() { "teclado_gamer.png" });
                await AddProductAsync("Silla Gamer", 980000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new
                List<string>() { "silla_gamer.png" });
                await AddProductAsync("Mouse Gamer", 132000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new
                List<string>() { "mouse_gamer.png" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string>
 images)
        {
            Product product = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };
            foreach (string? category in categories)
            {
                product.ProductCategories.Add(new ProductCategory
                {
                    Category = await
                _context.Categories.FirstOrDefaultAsync(c => c.Name == category)
                });
            }
            
            
            _context.Products.Add(product);
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName,
            string email, string phone, string address, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }
        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Tecnología" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Gamer" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Deportes" });
                _context.Categories.Add(new Category { Name = "Mascotas" });
                _context.Categories.Add(new Category { Name = "Apple" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { Name = "Colombia", 
                    States = new List<State>() 
                    { 
                        new State() { Name = "Antioquia", 
                            Cities = new List<City>() 
                            { 
                                new City() { Name = "Medellín" }, 
                                new City() { Name = "Itagüí" }, 
                                new City() { Name = "Envigado" }, 
                                new City() { Name = "Bello" }, 
                                new City() { Name = "Rionegro" }, 
                            } 
                        }, 
                        new State() { Name = "Bogotá", 
                            Cities = new List<City>() 
                            { new City() { Name = "Usaquen" }, 
                                new City() { Name = "Champinero" }, 
                                new City() { Name = "Santa fe" }, 
                                new City() { Name = "Useme" }, 
                                new City() { Name = "Bosa" }, 
                            } 
                        }, 
                    } 
                }); 
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                    new State()
                    {
                    Name = "Florida",
                    Cities = new List<City>() {
                    new City() { Name = "Orlando" },
                    new City() { Name = "Miami" },
                    new City() { Name = "Tampa" },
                    new City() { Name = "Fort Lauderdale" },
                    new City() { Name = "Key West" },
                    }
                    },
                    new State()
                    {
                    Name = "Texas",
                    Cities = new List<City>() {
                    new City() { Name = "Houston" },
                    new City() { Name = "San Antonio" },
                    new City() { Name = "Dallas" },
                    new City() { Name = "Austin" },
                    new City() { Name = "El Paso" },
                    }
                    },
                }
                });
            }
            await _context.SaveChangesAsync();
        }

    }
}


