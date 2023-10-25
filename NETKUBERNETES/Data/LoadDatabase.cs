using Microsoft.AspNetCore.Identity;
using NETKUBERNETES.Models;

namespace NETKUBERNETES.Data
{
    public class LoadDatabase
    {
        public static async Task InsertData(AppDbContext context, UserManager<Usuario> userManager)
        {
            if (!userManager.Users.Any()) {
                var usuario = new Usuario
                {
                    Nombre = "Luis",
                    Apellido = "Sanchez",
                    Email = "lesg.3322@gmail.com",
                    UserName = "alphamen01",
                    Telefono = "98142545"
                };

                await userManager.CreateAsync(usuario, "PasswordLesg123$");
            }

            if (!context.Inmuebles!.Any())
            {
                context.Inmuebles!.AddRange(

                        new Inmueble
                        {
                            Nombre = "Casa de Playa",
                            Direccion = "Av. El Sol 32",
                            Precio = 456M,
                            FechaCreacion = DateTime.Now
                        },

                        new Inmueble
                        {
                            Nombre = "Casa de Invierno",
                            Direccion = "Av. La roca 101",
                            Precio = 349M,
                            FechaCreacion = DateTime.Now
                        }
                  );
            }

            context.SaveChanges();
        }
            
    }
}
