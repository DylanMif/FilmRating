using Microsoft.VisualStudio.TestTools.UnitTesting;
using FilmRating.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TP3Console.Models.EntityFramework;
using FilmRating.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace FilmRating.Controllers.Tests
{
    [TestClass()]
    public class UtilisateursControllerTests
    {
        private UtilisateursController controller;
        private FilmDBContext context;
        [TestInitialize]
        public void Init()
        {
            var builder = new DbContextOptionsBuilder<FilmDBContext>().UseNpgsql("Server=localhost;port=5432;Database=RatingFilmsDB; uid=postgres;password=postgres;");
            context = new FilmDBContext(builder.Options);
            controller = new UtilisateursController(context);
        }

        [TestMethod()]
        public void GetUtilisateursTest()
        {
            // Arrange
            List<Utilisateur> expected = context.Utilisateurs.ToList();
            // Act
            var res = controller.GetUtilisateurs().Result;
            // Assert
            CollectionAssert.AreEqual(expected, res.Value.ToList(), "Les listes ne sont pas identiques");
        }

        [TestMethod()]
        public void GetUtilisateurByIdTest_Sucess()
        {
            // Arrange
            Utilisateur expected = context.Utilisateurs.Find(1);
            // Act
            var res = controller.GetUtilisateurById(1).Result;
            // Assert
            Assert.AreEqual(expected, res.Value);
        }

        [TestMethod()]
        public void GetUtilisateurByIdTest_NotFound()
        {
            // Act
            var res = controller.GetUtilisateurById(-1).Result;
            // Assert
            Assert.IsInstanceOfType(res.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void GetUtilisateurByEmailTest_Sucess()
        {
            // Arrange
            Utilisateur expected = context.Utilisateurs.FirstOrDefault(u => u.Mail == "lrudland3@360.cn");
            // Act
            var res = controller.GetUtilisateurByEmail("lrudland3@360.cn").Result;
            // Assert
            Assert.AreEqual(expected, res.Value);
        }

        [TestMethod()]
        public void GetUtilisateurByEmailTest_NotFound()
        {
            // Act
            var res = controller.GetUtilisateurByEmail("g").Result;
            // Assert
            Assert.IsInstanceOfType(res.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void PutUtilisateurTest()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Utilisateur userAtester = context.Utilisateurs.Find(1);
            userAtester.Mail = "machin" + chiffre + "@gmail.com";

            // Act
            var res = controller.PutUtilisateur(1, userAtester);

            // Arrange
            Utilisateur userMisAJour = context.Utilisateurs.Find(1);
            Assert.AreEqual(userAtester, userMisAJour);
        }

        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API 
            // ou remove du DbSet.
             Utilisateur userAtester = new Utilisateur()
             {
                 Nom = "MACHIN",
                 Prenom = "Luc",
                 Mobile = "0606070809",
                 Mail = "machin" + chiffre + "@gmail.com",
                 Pwd = "Toto1234!",
                 Rue = "Chemin de Bellevue",
                 CodePostal = "74940",
                 Ville = "Annecy-le-Vieux",
                 Pays = "France",
                 Latitude = null,
                 Longitude = null
             };
            // Act
            var result = controller.PostUtilisateur(userAtester).Result; // .Result pour appeler la méthode async de manière 
            // synchrone, afin d'attendre l’ajout
             // Assert
            Utilisateur? userRecupere = context.Utilisateurs.Where(u => u.Mail.ToUpper() ==
            userAtester.Mail.ToUpper()).FirstOrDefault(); // On récupère l'utilisateur créé directement dans la BD grace à son mail 
                        // unique
             // On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
             // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
            userAtester.UtilisateurId = userRecupere.UtilisateurId;
            Assert.AreEqual(userRecupere, userAtester, "Utilisateurs pas identiques");
        }

        [TestMethod()]
        public void DeleteUtilisateurTest()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            context.Utilisateurs.Add(userAtester);
            context.SaveChanges();

            // Act
            Utilisateur deletedUser = context.Utilisateurs.FirstOrDefault(u => u.Mail == userAtester.Mail);
            _ = controller.DeleteUtilisateur(deletedUser.UtilisateurId).Result;

            // Arrange
            Utilisateur res = context.Utilisateurs.FirstOrDefault(u => u.Mail == userAtester.Mail);
            Assert.IsNull(res, "utilisateur non supprimé");
        }
    }
}