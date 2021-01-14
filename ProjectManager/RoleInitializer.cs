using Microsoft.AspNetCore.Identity;
using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager
{
    public class RoleInitializer
    {
        public static async System.Threading.Tasks.Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            string email;
            string password;

            if (await roleManager.FindByNameAsync("supervisor") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("supervisor"));
            }
            if (await roleManager.FindByNameAsync("manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("manager"));
            }
            if (await roleManager.FindByNameAsync("employee") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("employee"));
            }

            email = "supervisor@project.com";
            password = "_Aa123456";
            if (await userManager.FindByNameAsync(email) == null)
            {
                Supervisor supervisor = new Supervisor { Email = email, UserName = email, Name = "supervisor", Surname = "supervisor", Patronymic = "supervisor" };
                IdentityResult result = await userManager.CreateAsync(supervisor, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(supervisor, "supervisor");
                }
            }

            email = "manager1@project.com";
            password = "_Aa123456";
            if (await userManager.FindByNameAsync(email) == null)
            {
                Manager manager = new Manager { Email = email, UserName = email, Name = "manager1", Surname = "manager1", Patronymic = "manager1" };
                IdentityResult result = await userManager.CreateAsync(manager, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager, "manager");
                }
            }

            email = "manager2@project.com";
            password = "_Aa123456";
            if (await userManager.FindByNameAsync(email) == null)
            {
                Manager manager = new Manager { Email = email, UserName = email, Name = "manager2", Surname = "manager2", Patronymic = "manager2" };
                IdentityResult result = await userManager.CreateAsync(manager, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager, "manager");
                }
            }

            email = "employee1@project.com";
            password = "_Aa123456";
            if (await userManager.FindByNameAsync(email) == null)
            {
                Employee employee = new Employee { Email = email, UserName = email, Name = "employee1", Surname = "employee1", Patronymic = "employee1" };
                IdentityResult result = await userManager.CreateAsync(employee, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employee, "employee");
                }
            }

            email = "employee2@project.com";
            password = "_Aa123456";
            if (await userManager.FindByNameAsync(email) == null)
            {
                Employee employee = new Employee { Email = email, UserName = email, Name = "employee2", Surname = "employee2", Patronymic = "employee2" };
                IdentityResult result = await userManager.CreateAsync(employee, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employee, "employee");
                }
            }
        }
    }
}
