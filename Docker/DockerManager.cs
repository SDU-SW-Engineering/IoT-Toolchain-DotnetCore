using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace BOSToolchain.Docker {
    public class DockerManager {
        public DockerClient Client { get; set; }

        public DockerManager() {
            switch (OperatingSystemTools.Identify()) {
                case OperatingSystemType.Windows:
                    Client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
                    break;
                case OperatingSystemType.MacOS:
                    Client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
                    break;
                case OperatingSystemType.Linux:
                    Client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
                    break;
                case OperatingSystemType.Other:
                    throw new Exception("Operating system not supported");
                default:
                    throw new Exception("Operating system identification error");
            }
        }

        public IList<ContainerListResponse> GetContainers() {
            return Client.Containers.ListContainersAsync(new ContainersListParameters() { Limit = 100 }).GetAwaiter().GetResult(); ;
        }

        public IList<ImagesListResponse> GetImages() {
            return Client.Images.ListImagesAsync(new ImagesListParameters()).GetAwaiter().GetResult();
        }
    }
}