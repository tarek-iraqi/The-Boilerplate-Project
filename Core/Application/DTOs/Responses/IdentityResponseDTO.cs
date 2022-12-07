using System;
using System.Collections.Generic;

namespace Application.DTOs;

public class IdentityResponseDTO
{
    public bool success { get; set; }
    public List<Tuple<string, string>> errors { get; set; }
}