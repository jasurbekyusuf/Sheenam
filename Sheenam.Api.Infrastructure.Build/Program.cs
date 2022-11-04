//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use Comfort and Pease
//===================================================

using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Clients;

var githubPipeline = new GithubPipeline
{
    Name = "Sheenam Build Pipline",

    OnEvents = new Events
    {
        PullRequest = new PullRequestEvent
        {
            Branches = new string[] { "master" }
        },

        Push = new PushEvent
        {
            Branches = new string[] { "master" }
        }
    },

    Jobs = new Jobs
    {
        Build = new BuildJob
        {
            RunsOn = BuildMachines.Windows2022,

            Steps = new List<GithubTask>
            {
                new CheckoutTaskV2
                {
                    Name = "Checking Out Code"
                },

                new SetupDotNetTaskV1
                {
                    Name = "Seting Up .NET",
                    TargetDotNetVersion = new TargetDotNetVersion
                    {
                        DotNetVersion = "6.0.402"
                    }
                },

                new RestoreTask
                {
                    Name = "Restarting Nutget Packages"
                },

                new DotNetBuildTask
                {
                    Name = "Building Project"
                },

                new TestTask
                {
                    Name = "Running Tests"
                }
            }
        }
    }
};


var client = new ADotNetClient();

client.SerializeAndWriteToFile(
    adoPipeline: githubPipeline,
    path: "../../../../.github/workflows/dotnet.yml");
