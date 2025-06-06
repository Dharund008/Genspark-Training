using NotifyAPI.Models;
using NotifyAPI.Contexts;
using NotifyAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NotifyAPI.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ManagementContext _context;

        public FileRepository(ManagementContext context)
        {
            _context = context;
        }

        public async Task<FileDocument> Add(FileDocument document)
        {
            _context.FileDocuments.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<IEnumerable<FileDocument>> GetAll()
        {
            return await _context.FileDocuments.ToListAsync();
        }
    }
}
