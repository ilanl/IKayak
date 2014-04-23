<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Trace.aspx.cs" Inherits="IKayak.Trace" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        a[disabled=noEvents]
        {
            display:none;
            color: red;
            font-weight: bold;
        }
        span.tracecontent b
        {
        }
        span.tracecontent
        {
            background-color: white;
            color: black;
            font: 10pt verdana, arial;
        }
        span.tracecontent table
        {
            clear: left;
            font: 10pt verdana, arial;
            cellspacing: 0;
            cellpadding: 0;
            margin-bottom: 25;
        }
        span.tracecontent tr.subhead
        {
            background-color: #cccccc;
        }
        span.tracecontent th
        {
            padding: 0,3,0,3;
        }
        span.tracecontent th.alt
        {
            background-color: black;
            color: white;
            padding: 3,3,2,3;
        }
        span.tracecontent td
        {
            color: black;
            padding: 0,3,0,3;
            text-align: left;
        }
        span.tracecontent td.err
        {
            color: red;
        }
        span.tracecontent tr.alt
        {
            background-color: #eeeeee;
        }
        span.tracecontent h1
        {
            font: 24pt verdana, arial;
            margin: 0,0,0,0;
        }
        span.tracecontent h2
        {
            font: 18pt verdana, arial;
            margin: 0,0,0,0;
        }
        span.tracecontent h3
        {
            font: 12pt verdana, arial;
            margin: 0,0,0,0;
        }
        span.tracecontent th a
        {
            color: darkblue;
            font: 8pt verdana, arial;
        }
        span.tracecontent a
        {
            color: darkblue;
            text-decoration: none;
        }
        span.tracecontent a:hover
        {
            color: darkblue;
            text-decoration: underline;
        }
        span.tracecontent div.outer
        {
            width: 90%;
            margin: 15,15,15,15;
        }
        span.tracecontent table.viewmenu td
        {
            background-color: #006699;
            color: white;
            padding: 0,5,0,5;
        }
        span.tracecontent table.viewmenu td.end
        {
            padding: 0,0,0,0;
        }
        span.tracecontent table.viewmenu a
        {
            color: white;
            font: 8pt verdana, arial;
        }
        span.tracecontent table.viewmenu a:hover
        {
            color: white;
            font: 8pt verdana, arial;
        }
        span.tracecontent a.tinylink
        {
            color: darkblue;
            background-color: black;
            font: 8pt verdana, arial;
            text-decoration: underline;
        }
        span.tracecontent a.link
        {
            color: darkblue;
            text-decoration: underline;
        }
        
        span.tracecontent div.buffer
        {
            padding-top: 7;
            padding-bottom: 17;
        }
        span.tracecontent .small
        {
            font: 8pt verdana, arial;
        }
        span.tracecontent table td
        {
            padding-right: 20;
        }
        span.tracecontent table td.nopad
        {
            padding-right: 5;
        }
        ul
        {
            width: 760px;
            margin-bottom: 20px;
            overflow: hidden;
        }
        li
        {
            margin-right: 20px;
            line-height: 1.5em;
            float: left;
            display: inline;
        }
        #double li
        {
            width: 50%;
        }
        /* 2 col */
        #triple li
        {
            width: 33.333%;
        }
        /* 3 col */
        #quad li
        {
            width: 25%;
        }
        /* 4 col */
        #six li
        {
            width: 16.666%;
        }
        
        /* 6 col */
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <span class="tracecontent" id="filter" runat="server" visible="false">
        <table cellspacing="0" cellpadding="0" style="width: 100%; border-collapse: collapse;">
            <tr>
                <td>
                    <h1>
                        Custom Trace</h1>
                </td>
            </tr>
            <tr>
                <td>
                    <h2>
                        Filter by:
                        <asp:TextBox ID="txtFilterIp" runat="server" Width="381px"></asp:TextBox>
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" /><h2>
                            <p>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <asp:Repeater ID="rptResults" runat="server">
            <HeaderTemplate>
                <table cellspacing="0" cellpadding="0" style="width: 100%; border-collapse: collapse;">
                    <tr>
                        <th class="alt" align="left" colspan="5">
                            <h3>
                                <b>Requests to this Application</b></h3>
                        </th>
                        <th class="alt" align="right">
                        </th>
                    </tr>
                    <tr class="subhead" align="left">
                        <th>
                            Ip Address
                        </th>
                        <th>
                            Time of Request
                        </th>
                        <th>
                            Title
                        </th>
                        <th>
                            Error Code
                        </th>
                        <th>
                            Took (ms)
                        </th>
                        <th>
                            &nbsp;
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="alt">
                    <td>
                        <%# Eval("ClientIp") %>
                    </td>
                    <td>
                        <%# Eval("TimeStamp") %>
                    </td>
                    <td>
                        <%# Eval("Title") %>
                    </td>
                    <td>
                        <%# Eval("Error") %>
                    </td>
                    <td>
                        <%# Eval("Elapsed") %>
                    </td>
                    <td>
                        <a disabled="<%# Eval("HasEvents") %>" href="Trace.aspx?id=<%# Eval("Id") %>" class="link">
                            <nobr>
                            View Details</a>
                    </td>
                </tr>
            </ItemTemplate>
            <SeparatorTemplate>
            </SeparatorTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </span><span class="tracecontent" id="log" runat="server" visible="false"><a href="Trace.aspx"
        style="position: absolute; border-bottom-width: 0px; margin-bottom: 0px; margin-top: 0px;
        bottom: 10px; top: 35px; right: 0px; width: 70px; left: 304px; height: 18px;">Back
    </a>
        <h1>
            Request Details
        </h1>
        <br>
        <asp:Repeater ID="requestDetails" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li><b>
                    <%# Eval("PropertyName") %>:</b>
                    <%# Eval("PropertyValue") %>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rtpLogLines" runat="server">
            <HeaderTemplate>
                <table cellspacing="2" cellpadding="1" style="width: 100%; border-collapse: collapse;">
                    <tr>
                        <th class="alt" align="left" colspan="10">
                            <h3>
                                <b>Trace Information</b>
                            </h3>
                        </th>
                    </tr>
                    <tr class="subhead" align="left">
                        <th>
                            Location
                        </th>
                        <th>
                            Level
                        </th>
                        <th style="margin-left: 50px; width: 1247px;">
                            Message
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("LocationInformation.ClassName") %>&nbsp (<%# Eval("LocationInformation.LineNumber") %>)
                    </td>
                    <td>
                        <%# Eval("Level.DisplayName")%>
                    </td>
                    <td>
                        <%# Eval("RenderedMessage") %>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="alt">
                    <td>
                        <%# Eval("LocationInformation.ClassName") %>&nbsp (<%# Eval("LocationInformation.LineNumber") %>)
                    </td>
                    <td>
                        <%# Eval("Level.DisplayName") %>
                    </td>
                    <td>
                        <%# Eval("RenderedMessage") %>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </span>
    </form>
</body>
</html>
