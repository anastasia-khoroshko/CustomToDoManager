using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomToDoManager.FastService;

namespace CustomToDoManager
{
    public static class Mapper
    {
        public static CustomTask ToCustomTask(this CustomToDoManager.SlowService.ToDoItem item)
        {
            return new CustomTask()
            {
                Name = item.Name,
                UserId = item.UserId,
                ToDoId = item.ToDoId,
                IsCompleted = item.IsCompleted
            };
        }

        public static CustomToDoManager.SlowService.ToDoItem ToToDoItem(this CustomTask item)
        {
            return new CustomToDoManager.SlowService.ToDoItem()
            {
                Name = item.Name,
                UserId = item.UserId,
                ToDoId = item.ToDoId,
                IsCompleted = item.IsCompleted
            };
        }
    }
}
