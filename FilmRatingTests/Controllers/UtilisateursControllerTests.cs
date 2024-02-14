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
using FilmRating.Repository;
using FilmRating.Models.DataManager;
using Humanizer;
using NuGet.ContentModel;
using Moq;

namespace FilmRating.Controllers.Tests
{
    [TestClass()]
    public class UtilisateursControllerTests
    {
        private UtilisateursController controller;
        private FilmDBContext context;
        private IDataRepository<Utilisateur> dataRepository;


        [TestInitialize]
        public void Init()
        {
            var builder = new DbContextOptionsBuilder<FilmDBContext>().UseNpgsql("Server=localhost;port=5432;Database=RatingFilmsDB; uid=postgres;password=postgres;");
            context = new FilmDBContext(builder.Options);
            dataRepository = new UtilisateurManager(context);
            controller = new UtilisateursController(dataRepository);
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
            Assert.IsNull(res.Value);
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
            Assert.IsNull(res.Value);
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
        public void GetUtilisateurById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurById(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value as Utilisateur);
        }

        [TestMethod]
        public void GetUtilisateurById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurById(0).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetUtilisateurByEmail_ExistingEmailPassed_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Bastard",
                Prenom = "Quentin",
                Mobile = "0654789632",
                Mail = "qbastard99@gmail.com",
                Pwd = "1234",
                Rue = "chemin des hutins",
                CodePostal = "74100",
                Ville = "Annemasse",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByStringAsync("qbastard99@gmail.com").Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurByEmail("qbastard99@gmail.com").Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value as Utilisateur);
        }

        [TestMethod]
        public void GetUtilisateurByEmail_UnknownEmailPassed_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurByEmail("qbastard@gmail.com").Result;
            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteUtilisateurTest_AvecMoq()
        {
            // Arrange
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 2,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(2).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.DeleteUtilisateur(2).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        [TestMethod]
        public void PutUtilisateurTestAvecMoq()
        {
            Utilisateur userEdit = new Utilisateur
            {
                UtilisateurId = 2,
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            Utilisateur userEdited = new Utilisateur
            {
                UtilisateurId = 2,
                Nom = "COLIN",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(2).Result).Returns(userEdit);
            var userController = new UtilisateursController(mockRepository.Object);

            // Act
            var actionResult = userController.PutUtilisateur(2, userEdited).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }

        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            Utilisateur user = new Utilisateur
            {
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var actionResult = userController.PostUtilisateur(user).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Utilisateur>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Utilisateur), "Pas un Utilisateur");
            user.UtilisateurId = ((Utilisateur)result.Value).UtilisateurId;
            Assert.AreEqual(user, (Utilisateur)result.Value, "Utilisateurs pas identiques");
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