﻿using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using DS.Application.DTO.MusicUser;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MusicUser.Queries;

public class GetUserForAuthorization
{
    public record GetUserQuery(string Token) : IRequest<Response>;

    public record Response(Domain.MusicUser User);

    public class Handler : IRequestHandler<GetUserQuery, Response>
    {
        private readonly MusicDbContext _context;

        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var token = new JwtSecurityToken(request.Token);
            Domain.MusicUser? user = await _context.MusicUsers.FindAsync(Guid.Parse(token.Id));

            if (user is null)
                throw new UnauthorizedException();

            return new Response(user);
        }
    }
}