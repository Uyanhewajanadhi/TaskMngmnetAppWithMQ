using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TaskService.Controllers;
using Moq;
using TaskService.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class UnitTestTaskService
    {
        public IQueryable<TasksDb> Data { get; set; } = new List<TasksDb>
            {
                new TasksDb { TaskId =0, TaskName = "Configure Kubernetes", Status = 1, EmpId=12 },
                new TasksDb { TaskId =1, TaskName = "Set Up RabbitMQ", Status = 0, EmpId=41 },
                new TasksDb { TaskId =2, TaskName = "Unit Test", Status = 2, EmpId=15  },
                new TasksDb { TaskId =3, TaskName = "Configure Docker", Status = 10, EmpId=12 },
                new TasksDb { TaskId =4, TaskName = "Take a Product Meeting", Status = 0, EmpId=41 },
                new TasksDb { TaskId =5, TaskName = "Participate in SCRUM", Status = 2, EmpId=15  },
            }.AsQueryable();
        public UnitTestTaskService()
        {
             
        }
        [TestMethod]
        public void GetAllTasks_by_name() 
        {         

            var mockSet = new Mock<DbSet<TasksDb>>();
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.Provider).Returns(Data.Provider);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.Expression).Returns(Data.Expression);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.ElementType).Returns(Data.ElementType);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.GetEnumerator()).Returns(Data.GetEnumerator());

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockSet.Object);

            var service = new TasksController(mockContext.Object);
            List<TasksDb> tasks;
            
            try
            {
                tasks = service.GetTasksNames();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
            
            
            Assert.AreEqual(6, tasks.Count);
            Assert.AreEqual("Configure Kubernetes", tasks[0].TaskName);
            Assert.AreEqual("Unit Test", tasks[2].TaskName);
        }

        [TestMethod]
        public void GetAllTasks_by_status()
        {
            var mockSet = new Mock<DbSet<TasksDb>>();
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.Provider).Returns(Data.Provider);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.Expression).Returns(Data.Expression);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.ElementType).Returns(Data.ElementType);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.GetEnumerator()).Returns(Data.GetEnumerator());

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockSet.Object);

            var service = new TasksController(mockContext.Object);
            List<TasksDb> tasks;

            try
            {
                tasks = service.GetTasksStatus(2);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

            Assert.AreEqual("Unit Test", tasks[0].TaskName);
            Assert.AreEqual("Participate in SCRUM", tasks[1].TaskName);
        }

        [TestMethod]
        public void GetAllTasks_by_Id()
        {
            var mockSet = new Mock<DbSet<TasksDb>>();
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.Provider).Returns(Data.Provider);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.Expression).Returns(Data.Expression);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.ElementType).Returns(Data.ElementType);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.GetEnumerator()).Returns(Data.GetEnumerator());

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockSet.Object);

            var service = new TasksController(mockContext.Object);
            TasksDb task;

            try
            {
                task = service.GetTasksId(4);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

            Assert.AreEqual("Take a Product Meeting", task.TaskName);
        }


        [TestMethod]
        public void PostTasks_by_Id()
        {
            var mockSet = new Mock<DbSet<TasksDb>>();
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.Provider).Returns(Data.Provider);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.Expression).Returns(Data.Expression);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.ElementType).Returns(Data.ElementType);
            mockSet.As<IQueryable<TasksDb>>().Setup(m => m.GetEnumerator()).Returns(Data.GetEnumerator());

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Tasks).Returns(mockSet.Object);

            var service = new TasksController(mockContext.Object);
            List<TasksDb> tasks;

            try
            {
                tasks = service.PostTasks();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


            Assert.AreEqual(6, tasks.Count);
            //Assert.AreEqual("Configure Kubernetes", tasks[0].TaskName);
            //Assert.AreEqual("BBB", tasks[1].TaskName);
            //Assert.AreEqual("ZZZ", tasks[2].TaskName);
        }
    }
      

}

