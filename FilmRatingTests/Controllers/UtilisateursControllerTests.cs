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
        public void GetUtilisateurByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUtilisateurByEmailTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutUtilisateurTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostUtilisateurTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteUtilisateurTest()
        {
            Assert.Fail();
        }
    }
}