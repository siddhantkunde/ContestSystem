using ContestSystem.Models;

namespace ContestSystem.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        // Prevent duplicate seeding
        if (context.Users.Any() && context.Contests.Any() && context.Questions.Any())
            return;

        // ========================
        // USERS
        // ========================
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { Username = "admin", Password = "123", Role = "Admin" },
                new User { Username = "vip", Password = "123", Role = "VIP" },
                new User { Username = "user", Password = "123", Role = "Normal" }
            );

            context.SaveChanges();
        }

        // ========================
        // CONTESTS
        // ========================
        if (!context.Contests.Any())
        {
            context.Contests.AddRange(
                new Contest
                {
                    Name = "Normal Quiz",
                    Description = "Basic general knowledge quiz",
                    AccessLevel = "Normal",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(1),
                    Prize = "Amazon Voucher"
                },
                new Contest
                {
                    Name = "VIP Exclusive Quiz",
                    Description = "Advanced level quiz for VIP users",
                    AccessLevel = "VIP",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(1),
                    Prize = "iPhone"
                }
            );

            context.SaveChanges();
        }

        // ========================
        // QUESTIONS + OPTIONS
        // ========================
        if (!context.Questions.Any())
        {
            var normalContest = context.Contests
                .FirstOrDefault(c => c.AccessLevel == "Normal");

            var vipContest = context.Contests
                .FirstOrDefault(c => c.AccessLevel == "VIP");

            if (normalContest == null || vipContest == null)
                return;

            // ===== NORMAL CONTEST QUESTIONS =====
            context.Questions.AddRange(

                // Single Select
                new Question
                {
                    Text = "What is 2 + 2?",
                    Type = "Single",
                    ContestId = normalContest.Id,
                    Options = new List<Option>
                    {
                        new Option { Text = "3", IsCorrect = false },
                        new Option { Text = "4", IsCorrect = true },
                        new Option { Text = "5", IsCorrect = false }
                    }
                },

                // True/False
                new Question
                {
                    Text = "Sun rises from the East?",
                    Type = "TrueFalse",
                    ContestId = normalContest.Id,
                    Options = new List<Option>
                    {
                        new Option { Text = "True", IsCorrect = true },
                        new Option { Text = "False", IsCorrect = false }
                    }
                },
                new Question
                {
                    Text = "Earth is flat?",
                    Type = "TrueFalse",
                    ContestId = vipContest.Id,
                    Options = new List<Option>
                    {
                        new Option { Text = "True", IsCorrect = false },
                        new Option { Text = "False", IsCorrect = true }
                    }
                }
            );

            // ===== VIP CONTEST QUESTIONS =====
            context.Questions.AddRange(

                // Multi Select
                new Question
                {
                    Text = "Select prime numbers",
                    Type = "Multi",
                    ContestId = vipContest.Id,
                    Options = new List<Option>
                    {
                        new Option { Text = "2", IsCorrect = true },
                        new Option { Text = "3", IsCorrect = true },
                        new Option { Text = "4", IsCorrect = false },
                        new Option { Text = "5", IsCorrect = true }
                    }
                }
            );

            context.SaveChanges();
        }
    }
}