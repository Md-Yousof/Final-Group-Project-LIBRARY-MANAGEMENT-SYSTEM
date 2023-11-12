using AutoMapper.Execution;
using LibraryAPI_R53_A.Core;
using LibraryAPI_R53_A.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryAPI_R53_A.Persistence.services
{
    public class ContextSeedService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ContextSeedService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeContextAsync()
        {
            if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
            {
                await _context.Database.MigrateAsync();
            }
            #region Seed Value for role. R-Step 02

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.AdminRole });
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.ManagerRole });
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.UserRole });
            }

            if (!_userManager.Users.AnyAsync().GetAwaiter().GetResult())
            {
                var admin = new ApplicationUser
                {
                    UserName = SD.AdminUserName,
                    Email = SD.AdminUserName,
                    EmailConfirmed = true,
                    IsActive = true,
                };
                await _userManager.CreateAsync(admin, "123456");
                await _userManager.AddToRolesAsync(admin, new[] { SD.AdminRole, SD.ManagerRole, SD.UserRole });
                await _userManager.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.GivenName, admin.UserName)
                });

                var manager = new ApplicationUser
                {
                    UserName = "manager",
                    Email = "manager@mail.com",
                    EmailConfirmed = true,
                    IsActive = true,
                };
                await _userManager.CreateAsync(manager, "123456");
                await _userManager.AddToRoleAsync(manager, SD.ManagerRole);
                await _userManager.AddClaimsAsync(manager, new Claim[]
                {
                    new Claim(ClaimTypes.Email, manager.Email),
                    new Claim(ClaimTypes.GivenName, manager.UserName)
                });

                var user = new ApplicationUser
                {
                    UserName = "user",
                    Email = "user@mail.com",
                    EmailConfirmed = true,
                    IsActive = true,
                };
                await _userManager.CreateAsync(user, "123456");
                await _userManager.AddToRoleAsync(user, SD.UserRole);
                await _userManager.AddClaimsAsync(user, new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.UserName)
                });

                var subscribeUser = new ApplicationUser
                {
                    UserName = "subscribeUser",
                    Email = "subscribeuser@mail.com",
                    EmailConfirmed = true,
                    IsActive = true,
                };
                await _userManager.CreateAsync(subscribeUser, "123456");
                await _userManager.AddToRoleAsync(subscribeUser, SD.UserRole);
                await _userManager.AddClaimsAsync(subscribeUser, new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.UserName)
                });
            }
            #endregion

            if (!_context.Publishers.Any())
            {
                // Seed data for the Publisher table here
                var publishers = new List<Publisher>
                {
                    new Publisher
                    {
                        PublisherName = "Publisher 1",
                        Address = "123 Main St",
                        Email = "publisher1@example.com",
                        PhoneNumber = "555-123-4567",
                        IsActive = true,
                    },
                    new Publisher
                    {
                        PublisherName = "Publisher 2",
                        Address = "124 Main St",
                        Email = "publisher2@example.com",
                        PhoneNumber = "556-123-4567",
                        IsActive = true,
                    }
                };
                _context.Publishers.AddRange(publishers);
                await _context.SaveChangesAsync();
            }

            if (!_context.Authors.Any())
            {
                // Seed data for the Author table here
                var authors = new List<Author>
                {
                    new Author
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        BirthDate = new DateTime(1980, 5, 15),
                        Biography = "A famous author from the Bangladesh.",
                        Email = "johndoe@example.com",
                        Phone = "555-123-4567",
                        IsActive = true
                    }
                };
                _context.Authors.AddRange(authors);
                await _context.SaveChangesAsync();

            }

            if (!_context.BookFloors.Any())
            {
                var bookFloors = new List<BookFloor>
                {
                    new BookFloor
                    {
                        BookFloorName = "Floor 1",
                        IsActive = true
                    },
                    new BookFloor
                    {
                        BookFloorName = "Floor 2",
                        IsActive = true
                    }
                };
                _context.BookFloors.AddRange(bookFloors);
                await _context.SaveChangesAsync();

            }

            if (!_context.Categorys.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        CategoryName = "Literature",
                        DDCCode = "800-899",
                        IsActive = true,
                        Subcategories = new List<Subcategory>
                        {
                            new Subcategory {DDCCode="823", Name = "English Fiction", IsActive=true }
                        }
                    }

                };
                _context.Categorys.AddRange(categories);
                await _context.SaveChangesAsync();

            }

            if (!_context.SubscriptionPlans.Any())
            {
                var subscriptionPlans = new List<SubscriptionPlan>
                {
                    new SubscriptionPlan
                    {
                        PlanName = "Basic Plan",
                        PlanDescription = "Can borrow Book that are not more than 1000",
                        PlanPrice = 1000.00M,
                        IsActive = true,
                        MonthlyFee = 100M,
                        MaxAllowedBookPrice = 1000M,
                    },
                     new SubscriptionPlan
                    {
                        PlanName = "Student Plan",
                        PlanDescription = "Can borrow Book that are not more than 3000",
                        PlanPrice = 3000.00M,
                        IsActive = true,
                        MonthlyFee = 100M,
                        MaxAllowedBookPrice = 3000M,
                    },
                      new SubscriptionPlan
                    {
                        PlanName = "Premium Plan",
                        PlanDescription = "Can borrow Book that are not more than 5000",
                        PlanPrice = 5000.00M,
                        IsActive = true,
                        MonthlyFee = 100M,
                        MaxAllowedBookPrice = 5000M,
                    }
                };
                _context.SubscriptionPlans.AddRange(subscriptionPlans);
                await _context.SaveChangesAsync();

            }

            if (!_context.Shelfs.Any())
            {
                var shelves = new List<Shelf>
                {
                    new Shelf
                    {
                        ShelfName = "Shelf 1",
                        IsActive = true,
                        BookFloorId = 1,
                        BookRacks = new List<BookRack>
                        {
                            new BookRack
                            {
                                BookRackName= "Rack 1"
                            }
                        }
                    }
                };
                _context.Shelfs.AddRange(shelves);
                await _context.SaveChangesAsync();

            }

            if (!_context.Books.Any())
            {
                var books = new List<Book>
                {
                    new Book
                    {
                        Title = "The Adventures of Sherlock Holmes",
                        ISBN = "978-1-234567-89-0",
                        PublisherId = 1,
                        PublishedYear = new DateTime(1892, 10, 14),
                        Edition = "1st Edition",
                        TotalCopies = 2,
                        Language = "English",
                        Description = "Author: John Doe. " +
                        "The Adventures of Sherlock Holmes is a collection of twelve short stories " +
                                      "featuring Sherlock Holmes, a fictional detective. The stories are the " +
                                      "first appearance of Holmes in book form and are widely considered some of " +
                                      "the finest short stories in the mystery genre.",
                        BookPrice = 500.00M,
                        DDCCode = "Fiction(823.8)",
                        IsActive = true,
                        IsDigital = false,
                        PublisherAgreement = false,
                        CategoryId = 1,
                        CoverFileName="http://localhost:5154/uploads/45b6a6a6-22a2-44f0-b346-c116ca126dd9_The_AdventureOf_SherlockHolmes.jpg",
                        BookAuthor = new List<BookAuthor>
                        {
                            new BookAuthor { AuthorId =1}
                        }
                    }
                };
                _context.Books.AddRange(books);
                await _context.SaveChangesAsync();

            }

            if (!_context.Copies.Any())
            {
                var bookCopies = new List<BookCopy>
                {
                    new BookCopy
                    {
                        CallNumber = "11-823.8-T-1",
                        IsAvailable = true,
                        IsActive = true,
                        condition = BookCondition.Good,
                        BookId = 1,
                        DDC = "Fiction(823.8)",
                        ShelfId = 1,
                    },
                    new BookCopy
                    {
                        CallNumber = "11-823.8-T-12",
                        IsAvailable = true,
                        IsActive = true,
                        condition = BookCondition.Good,
                        BookId = 1,
                        DDC = "Fiction(823.8)",
                        ShelfId = 1,
                    }
                };
                _context.Copies.AddRange(bookCopies);
                await _context.SaveChangesAsync();

            }


        }

    }
}
