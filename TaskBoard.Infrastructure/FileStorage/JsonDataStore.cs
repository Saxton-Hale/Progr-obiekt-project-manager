using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;
using TaskStatus = TaskBoard.Domain.Enums.TaskStatus;


namespace TaskBoard.Infrastructure.FileStorage
{
    internal class JsonDataStore
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        public void SaveBoards(IEnumerable<Board> boards, string path)
        {
            if (boards is null) throw new ArgumentNullException(nameof(boards));
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("PAth cannot be empty", nameof(path));

            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);

            var dto = boards.Select(BoardDto.FromDomain).ToList();
            var json = JsonSerializer.Serialize(dto, Options);

            File.WriteAllText(path, json, Encoding.UTF8);
        }

        public List<Board> LoadBoards(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path cannot be empty", nameof(path));
            if (!File.Exists(path)) return new List<Board>();

            var json = File.ReadAllText(path, Encoding.UTF8);
            var dtoBoards = JsonSerializer.Deserialize<List<BoardDto>>(json, Options) ?? new List<BoardDto>();

            return dtoBoards.Select(ToDomain).ToList();
        }

        //mapping dto => domain
        private static Board ToDomain(BoardDto dto)
        {
            var board = new Board(dto.Name);

            foreach(var colDto in dto.Columns)
            {
                board.AddColumn(colDto.Name);
                var column = board.Columns.Last();

                foreach(var taskDto in colDto.Tasks)
                {
                    var task = CreateTask(taskDto);

                    task.Description = taskDto.Description;
                    task.Status = taskDto.Status;
                    task.Priority = taskDto.Priority;
                    task.Id = taskDto.Id;

                    if(taskDto.DueDate.HasValue)
                    {
                        if (task.DueDate != taskDto.DueDate) 
                            task.ChangeDueDate(taskDto.DueDate);
                    }

                    column.AddTask(task);
                }
            }

            return board;
        }

        private static TaskItem CreateTask(TaskItemDto dto)
        {
            return dto.Type switch
            {
                "BugTask" => CreateWithOptionalDueDate<BugTask>(dto.Title, dto.DueDate),
                "FeatureTask" => CreateWithOptionalDueDate<FeatureTask>(dto.Title, dto.DueDate),
                _ => CreateWithOptionalDueDate<FeatureTask>(dto.Title, dto.DueDate) // fallback
            };
        }

        private static TaskItem CreateWithOptionalDueDate<TTask>(string title, DateTime? dueDate)
            where TTask : TaskItem
        {
            var type = typeof(TTask);

            var ctor2 = type.GetConstructor(new[] { typeof(string), typeof(DateTime?) });
            if (ctor2 != null)
                return (TaskItem)ctor2.Invoke(new object?[] { title, dueDate });

            if (dueDate.HasValue)
            {
                var ctorDateTime = type.GetConstructor(new[] { typeof(string), typeof(DateTime) });
                if (ctorDateTime != null)
                    return (TaskItem)ctorDateTime.Invoke(new object[] { title, dueDate.Value });
            }

            var ctor1 = type.GetConstructor(new[] { typeof(string) });
            if (ctor1 != null)
                return (TaskItem)ctor1.Invoke(new object[] { title });

            throw new InvalidOperationException($"No suitable constructor found for task type: {type.Name}");
        }

        //modele dto
        private sealed class BoardDto
        {
            public string Name { get; set; } = string.Empty;
            public List<ColumnDto> Columns { get; set; } = new();

            public static BoardDto FromDomain(Board board)
            {
                return new BoardDto
                {
                    Name = board.Name,
                    Columns = board.Columns.Select(ColumnDto.FromDomain).ToList()
                };
            }
        }

        private sealed class ColumnDto
        {
            public string Name { get; set; } = string.Empty;
            public List<TaskItemDto> Tasks { get; set; } = new();

            public static ColumnDto FromDomain(Column column)
            {
                return new ColumnDto
                {
                    Name = column.Name,
                    Tasks = column.Tasks.Select(TaskItemDto.FromDomain).ToList()
                };
            }
        }

        private sealed class TaskItemDto
        {
            public string Type { get; set; } = string.Empty;
            public Guid Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public TaskStatus Status { get; set; }
            public TaskPriority Priority { get; set; }
            public DateTime? DueDate { get; set; }

            public static TaskItemDto FromDomain(TaskItem task)
            {
                return new TaskItemDto
                {
                    Type = task.GetType().Name,
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    Priority = task.Priority,
                    DueDate = task.DueDate
                };
            }
        }
    }
}
