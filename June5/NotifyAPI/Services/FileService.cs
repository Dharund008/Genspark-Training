using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotifyAPI.Contexts;
using NotifyAPI.Interfaces;
using NotifyAPI.Models;
using NotifyAPI.Models.DTO;
using NotifyAPI.Repositories;
using NotifyAPI.Misc;
using Microsoft.AspNetCore.SignalR;

namespace NotifyAPI.Services
{
    public class FileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<NotificationHub> _hubContext;

        public FileService(IFileRepository fileRepository, IWebHostEnvironment env, IHubContext<NotificationHub> hubContext)
        {
            _fileRepository = fileRepository;
            _env = env;
            _hubContext = hubContext;
        }

        public async Task UploadFileAsync(FileUploadDTO dto, string uploadedBy)
        {
            var uploadsFolder = Path.Combine(_env.ContentRootPath, "UploadedDocs");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, dto.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var document = new FileDocument
            {
                FileName = dto.File.FileName,
                FilePath = filePath,
                UploadedBy = uploadedBy
            };

            await _fileRepository.Add(document);

            //notifying
             await _hubContext.Clients.All.SendAsync("ReceivedNotification", $"New file uploaded by {uploadedBy}: {dto.File.FileName}");
        }
    }
}