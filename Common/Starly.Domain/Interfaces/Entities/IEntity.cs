﻿namespace Starly.Domain.Interfaces.Entities;

public interface IEntity<T>
{
    T Id { get; set; }
}
