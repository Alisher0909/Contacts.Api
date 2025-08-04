using AutoMapper;
using ContactsApi.Models;
using ContactsApi.Dtos;

namespace ContactsApi.Services;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<ContactDto, Contact>();
        CreateMap<CreateContactDto, CreateContact>();
        CreateMap<UpdateContactDto, UpdateContact>();
        CreateMap<PatchContactDto, PatchContact>();

        CreateMap<Contact, ContactDto>();
        CreateMap<Contact, PatchContactDto>();
        CreateMap<CreateContact, Contact>();
        CreateMap<UpdateContact, Contact>();
        CreateMap<PatchContact, Contact>();
    }
}