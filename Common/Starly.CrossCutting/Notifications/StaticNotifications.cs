namespace Starly.CrossCutting.Notifications;

public static class StaticNotifications
{
    #region [Users]
    public static Notification InvalidCredentials = new("InvalidCredentials", "Credenciais invalidas!");
    public static Notification UserAlreadyExists = new("UserAlreadyExists", "Usuario já cadastrado!");
    public static Notification UserNotFound = new("InvalidUser", "Usuario não encontrado!");
    public static Notification RevokeToken = new("RevokeToken", "Token revogado com sucesso!");
    public static Notification InvalidToken = new("InvalidToken", "Token invalido!");
    public static Notification UserCreated = new ("UserCreated", "Usuario criado com sucesso!");
    public static Notification UsernameAlreadyExists = new("UsernameAlreadyExists", "Username já está sendo utilizado!");
    public static Notification UserEdited = new("UserEdited", "Usuario editado com sucesso!");
    public static Notification PasswordChanged = new("PasswordChanged", "Senha alterada com sucesso!");
    public static Notification PhotoUploaded = new("PhotoUploaded", "Upload da foto realizado com sucesso!");
    public static Notification UserDeleted = new("UserDeleted", "Usuario removido com sucesso!");
    public static Notification UserActivated = new("UserActivated", "Ativação de usuário alterada com sucesso!");
    #endregion

    #region [Business]
    public static Notification BusinessSuccess = new("BusinessSuccess", "Empresa inserida com sucesso!");
    public static Notification BusinessUpdated = new("BusinessUpdated", "Empresa atualizada com sucesso!");
    public static Notification BusinessError = new("BusinessError", "Erro ao inserir empresa!");
    public static Notification BusinessDeleted = new("BusinessDeleted", "Empresa deletada com sucesso!");
    public static Notification BusinessNotFound = new("BusinessNotFound", "Empresa não encontrada!");
    public static Notification BusinessPhotoNotFound = new("BusinessPhotoNotFound", "Foto não encontrada!");
    public static Notification BusinessPhotoUploaded = new("ReviewPhotoUploaded", "Foto enviadas com sucesso!");
    public static Notification BusinessPhotoDeleted = new("ReviewPhotoDeleted", "Foto deletada com sucesso!");
    #endregion

    #region [Category]
    public static Notification CategorySuccess = new("CategorySuccess", "Categoria inserida com sucesso!");
    public static Notification CategoryUpdated = new("CategoryUpdated", "Categoria atualizada com sucesso!");
    public static Notification CategoryError = new("CategoryError", "Erro ao inserir categoria!");
    public static Notification CategoryNotFound = new("CategoryNotFound", "Categoria não encontrada!");
    public static Notification CategoryDeleted = new("CategoryDeleted", "Categoria deletada com sucesso!");
    #endregion

    #region [Review]
    public static Notification ReviewError = new("ReviewError", "Erro ao inserir avaliação!");
    public static Notification ReviewSuccess = new("ReviewSuccess", "Avaliação inserida com sucesso!");
    public static Notification ReviewNotFound = new("ReviewNotFound", "Avaliação não encontrada!");
    public static Notification ReviewDeleted = new("ReviewDeleted", "Avaliação deletada com sucesso!");
    public static Notification ReviewUpdated = new("ReviewUpdated", "Avaliação atualizada com sucesso!");
    public static Notification ReviewPhotoUploaded = new("ReviewPhotoUploaded", "Foto enviadas com sucesso!");
    public static Notification ReviewPhotoDeleted = new("ReviewPhotoDeleted", "Foto deletada com sucesso!");
    #endregion
}
