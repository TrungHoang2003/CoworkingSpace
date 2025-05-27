using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Collections.CQRS.Commands;

public sealed record AddSpaceToCollection(int CollectionId, int SpaceId):IRequest<Result>;

public class AddToCollectionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddSpaceToCollection, Result>
{
    public async Task<Result> Handle(AddSpaceToCollection request, CancellationToken cancellationToken)
    {
        var collection = await unitOfWork.Collection.GetById(request.CollectionId);
        if (collection == null) return CollectionErrors.CollectionNotFound;
        
        var exist= await unitOfWork.Space.FindById(request.SpaceId);
        if (!exist) return SpaceErrors.SpaceNotFound;

        var spaceCollection = new SpaceCollection
        {
            CollectionId = request.CollectionId,
            SpaceId = request.SpaceId
        };
        await unitOfWork.SpaceCollection.Create(spaceCollection);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}