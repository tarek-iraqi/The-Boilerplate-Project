using Helpers.Abstractions;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Localization;
using System;
using System.Collections.Generic;

namespace Domain.ValueObjects;

public class Name : ValueObject
{
    public string First { get; }
    public string Last { get; }
    protected Name()
    {

    }
    private Name(string first, string last)
    {
        First = first;
        Last = last;
    }

    public static Name Create(string firstname, string lastname)
    {
        if (string.IsNullOrWhiteSpace(firstname))
            throw new AppCustomException(ErrorStatusCodes.BadRequest,
                new List<Tuple<string, string>> {
                    new(nameof(firstname), LocalizationKeys.FirstNameRequired)});

        if (string.IsNullOrWhiteSpace(lastname))
            throw new AppCustomException(ErrorStatusCodes.BadRequest,
                new List<Tuple<string, string>> {
                    new(nameof(firstname), LocalizationKeys.FirstNameRequired)});

        firstname = firstname.Trim();
        lastname = lastname.Trim();

        return new Name(firstname, lastname);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return First;
        yield return Last;
    }
}