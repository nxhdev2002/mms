using System.Collections.Generic;
using Abp;
using prod.Chat.Dto;
using prod.Dto;

namespace prod.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
