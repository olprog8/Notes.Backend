using System.Threading.Tasks;
using System.Threading;
using Notes.Application.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Application.Common.Exceptions;

using MediatR;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class GetNoteDetailsHandler: IRequestHandler<GetNoteDetailsQuery, NoteDetailsVM>
    {
        private readonly INotesDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetNoteDetailsHandler(INotesDbContext dbContext, IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<NoteDetailsVM> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
        {

            var entity = await _dbContext.Notes.FirstOrDefaultAsync(note =>
                                                                    note.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.Id)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            return _mapper.Map<NoteDetailsVM>(entity);

        }

    }
}
