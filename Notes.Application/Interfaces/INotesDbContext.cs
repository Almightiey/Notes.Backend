using Microsoft.EntityFrameworkCore;
using Notes.Domain;


namespace Notes.Application.Interfaces;

public interface INotesDbContext
{
    DbSet<Note> Notes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
