using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Collections.CQRS.Commands;
public sealed record RemoveSpaceFromCollection(int CollectionId, int SpaceId):IRequest<Result>;

public class RemoveFromCollectionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveSpaceFromCollection, Result>
{
    public async Task<Result> Handle(RemoveSpaceFromCollection request, CancellationToken cancellationToken)
    {
        var collection = await unitOfWork.Collection.GetById(request.CollectionId);
        if (collection == null) return CollectionErrors.CollectionNotFound;

        var exist = await unitOfWork.Space.FindById(request.SpaceId);
        if (!exist) return SpaceErrors.SpaceNotFound;

        var spaceCollection = await unitOfWork.SpaceCollection.GetById(request.CollectionId, request.SpaceId);
        if (spaceCollection == null) return CollectionErrors.SpaceNotInCollection;
        await unitOfWork.SpaceCollection.Delete(spaceCollection);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
