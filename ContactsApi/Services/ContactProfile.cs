using AutoMapper;
using ContactsApi.Dtos;
using ContactsApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactsApi.Services;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<CreateContactDto, CreateContact>();
        CreateMap<UpdateContactDto, UpdateContact>();
        CreateMap<ContactDto, Models.Contact>();

        CreateMap<CreateContact, Models.Contact>();
        CreateMap<Models.Contact, ContactDto>();
        CreateMap<UpdateContact, Models.Contact>();
        CreateMap<PatchContactDto, PatchContact>();
        CreateMap<PatchContact, Models.Contact>();

        CreateMap<CreateContact, Entities.Contact>();
        CreateMap<Entities.Contact, Models.Contact>();
        CreateMap<UpdateContact, Entities.Contact>();
        CreateMap<PatchContact, Entities.Contact>();
        CreateMap<Entities.Contact, UpdateContact>();
    }
}